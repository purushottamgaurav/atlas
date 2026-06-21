using DotNetFunctions.Models;
using DotNetFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DotNetFunctions.Functions;

// ─── Queue Trigger ────────────────────────────────────────────────────────────
// Use case: Async order processing pipeline.
//
// When PlaceOrder (HTTP) drops a message on "orders-queue", this function picks
// it up and runs the payment + inventory checks without blocking the HTTP caller.
// If processing fails, Azure Storage Queues automatically retries (up to 5 times)
// then moves the message to "orders-queue-poison" for manual inspection.
// ─────────────────────────────────────────────────────────────────────────────

public class OrderQueueFunction
{
    private readonly IOrderService _orders;
    private readonly ILogger<OrderQueueFunction> _logger;

    public OrderQueueFunction(IOrderService orders, ILogger<OrderQueueFunction> logger)
    {
        _orders = orders;
        _logger = logger;
    }

    [Function("ProcessOrder")]
    public async Task Run(
        [QueueTrigger("orders-queue", Connection = "AzureWebJobsStorage")] string message,
        FunctionContext context)
    {
        var order = JsonSerializer.Deserialize<Order>(message, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (order == null)
        {
            _logger.LogWarning("Received malformed order message — skipping.");
            return;
        }

        _logger.LogInformation("Processing order {OrderId} for {Email}", order.OrderId, order.CustomerEmail);

        _orders.UpdateStatus(order.OrderId, OrderStatus.Processing);

        // Simulate payment gateway call
        var paymentApproved = await SimulatePaymentAsync(order);
        if (!paymentApproved)
        {
            _logger.LogWarning("Payment declined for order {OrderId}", order.OrderId);
            _orders.UpdateStatus(order.OrderId, OrderStatus.Cancelled);
            return;
        }

        // Simulate inventory reservation
        var inventoryOk = await SimulateInventoryReservationAsync(order);
        if (!inventoryOk)
        {
            _logger.LogWarning("Inventory unavailable for order {OrderId} — cancelling", order.OrderId);
            _orders.UpdateStatus(order.OrderId, OrderStatus.Cancelled);
            return;
        }

        // Hand off to fulfilment — in prod, this would publish to Service Bus
        _logger.LogInformation("Order {OrderId} queued for fulfilment — total £{Total:F2}", order.OrderId, order.TotalAmount);
    }

    private static Task<bool> SimulatePaymentAsync(Order order)
    {
        // In production: call Stripe / Adyen / Braintree
        var approved = order.TotalAmount < 10_000m; // reject suspiciously large orders
        return Task.FromResult(approved);
    }

    private static Task<bool> SimulateInventoryReservationAsync(Order order)
    {
        // In production: call inventory microservice
        var allInStock = order.Items.All(i => i.Quantity <= 100);
        return Task.FromResult(allInStock);
    }
}
