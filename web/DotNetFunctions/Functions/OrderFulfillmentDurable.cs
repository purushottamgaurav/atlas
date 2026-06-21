using DotNetFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DotNetFunctions.Functions;

// ─── Durable Functions ────────────────────────────────────────────────────────
// Use case: Multi-step order fulfilment workflow.
//
// Durable Functions solves the problem of coordinating long-running, stateful
// workflows without managing state yourself. The orchestrator checkpoints after
// every activity, so if the host restarts mid-workflow, execution resumes from
// where it left off — not from the beginning.
//
// Workflow steps (sequential):
//   1. ValidateOrder    — check order exists and hasn't been cancelled
//   2. ProcessPayment   — charge the customer
//   3. UpdateInventory  — deduct stock
//   4. NotifyCustomer   — send confirmation email
//
// The HTTP starter returns check-status URLs so the caller can poll for progress.
// ─────────────────────────────────────────────────────────────────────────────

public class OrderFulfillmentDurable
{
    private readonly IOrderService _orders;
    private readonly ILogger<OrderFulfillmentDurable> _logger;

    public OrderFulfillmentDurable(IOrderService orders, ILogger<OrderFulfillmentDurable> logger)
    {
        _orders = orders;
        _logger = logger;
    }

    // ── HTTP Starter ──────────────────────────────────────────────────────────
    // POST /api/fulfill/{orderId}
    // Kicks off a new orchestration instance and returns status-check URLs.
    [Function("StartOrderFulfillment")]
    public async Task<HttpResponseData> StartFulfillment(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "fulfill/{orderId}")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        string orderId,
        FunctionContext executionContext)
    {
        _logger.LogInformation("Starting fulfillment orchestration for order {OrderId}", orderId);

        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            nameof(FulfillOrderOrchestrator), input: orderId);

        _logger.LogInformation("Orchestration started: instance {InstanceId}", instanceId);

        // Returns 202 Accepted with Location header + statusQueryGetUri / sendEventPostUri
        return await client.CreateCheckStatusResponseAsync(req, instanceId);
    }

    // ── Orchestrator ──────────────────────────────────────────────────────────
    // Orchestrators must be deterministic — no random values, no DateTime.Now,
    // no direct async I/O. Use context.CurrentUtcDateTime for timestamps.
    [Function(nameof(FulfillOrderOrchestrator))]
    public static async Task<FulfillmentResult> FulfillOrderOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var orderId = context.GetInput<string>()!;
        var logger = context.CreateReplaySafeLogger(nameof(FulfillOrderOrchestrator));

        logger.LogInformation("Orchestrating fulfilment for order {OrderId}", orderId);

        // Step 1: Validate
        var isValid = await context.CallActivityAsync<bool>(nameof(ValidateOrderActivity), orderId);
        if (!isValid)
            return new FulfillmentResult(orderId, false, "Order validation failed — order may not exist or is cancelled.");

        // Step 2: Payment
        var paymentOk = await context.CallActivityAsync<bool>(nameof(ProcessPaymentActivity), orderId);
        if (!paymentOk)
            return new FulfillmentResult(orderId, false, "Payment processing failed.");

        // Step 3: Inventory (can run in parallel with Step 4 — shown here as sequential for clarity)
        await context.CallActivityAsync(nameof(UpdateInventoryActivity), orderId);

        // Step 4: Notify customer
        await context.CallActivityAsync(nameof(NotifyCustomerActivity), orderId);

        return new FulfillmentResult(orderId, true, "Order fulfilled successfully.");
    }

    // ── Activities ────────────────────────────────────────────────────────────
    // Activities are the actual units of work. They can call external services,
    // write to databases, send emails, etc. They run exactly-once per orchestration step.

    [Function(nameof(ValidateOrderActivity))]
    public bool ValidateOrderActivity(
        [ActivityTrigger] string orderId,
        FunctionContext context)
    {
        _logger.LogInformation("[Activity] Validating order {OrderId}", orderId);
        var order = _orders.GetById(orderId);

        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found", orderId);
            return false;
        }

        if (order.Status == Models.OrderStatus.Cancelled)
        {
            _logger.LogWarning("Order {OrderId} is already cancelled", orderId);
            return false;
        }

        return true;
    }

    [Function(nameof(ProcessPaymentActivity))]
    public bool ProcessPaymentActivity(
        [ActivityTrigger] string orderId,
        FunctionContext context)
    {
        _logger.LogInformation("[Activity] Processing payment for order {OrderId}", orderId);
        var order = _orders.GetById(orderId);
        if (order == null) return false;

        // In production: call Stripe, Adyen, Braintree SDK here
        var charged = order.TotalAmount > 0;
        _logger.LogInformation("Payment {Result} for order {OrderId} — £{Total:F2}",
            charged ? "approved" : "declined", orderId, order.TotalAmount);

        return charged;
    }

    [Function(nameof(UpdateInventoryActivity))]
    public void UpdateInventoryActivity(
        [ActivityTrigger] string orderId,
        FunctionContext context)
    {
        _logger.LogInformation("[Activity] Updating inventory for order {OrderId}", orderId);
        var order = _orders.GetById(orderId);
        if (order == null) return;

        // In production: call inventory microservice to deduct stock
        foreach (var item in order.Items)
            _logger.LogInformation("  Deducted {Qty}x '{Product}' from stock", item.Quantity, item.ProductName);
    }

    [Function(nameof(NotifyCustomerActivity))]
    public void NotifyCustomerActivity(
        [ActivityTrigger] string orderId,
        FunctionContext context)
    {
        _logger.LogInformation("[Activity] Notifying customer for order {OrderId}", orderId);
        var order = _orders.GetById(orderId);
        if (order == null) return;

        // In production: send via SendGrid / Azure Communication Services
        _logger.LogInformation(
            "  Confirmation email sent to {Email} for order {OrderId} — £{Total:F2}",
            order.CustomerEmail, order.OrderId, order.TotalAmount);

        _orders.UpdateStatus(orderId, Models.OrderStatus.Processing);
    }
}

public record FulfillmentResult(string OrderId, bool Success, string Message);
