using DotNetFunctions.Models;
using DotNetFunctions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DotNetFunctions.Functions;

// ─── HTTP Trigger ─────────────────────────────────────────────────────────────
// Use case: REST API for the storefront.
//   GET  /api/orders/{orderId}  — query an order's status
//   POST /api/orders            — place a new order (enqueues it for async processing)
//
// Pattern: MultiOutput — returns both an HTTP response AND a queue message
// from a single function so placing an order immediately queues it for processing.
// ─────────────────────────────────────────────────────────────────────────────

public class OrdersHttpFunction
{
    private readonly IOrderService _orders;
    private readonly ILogger<OrdersHttpFunction> _logger;

    public OrdersHttpFunction(IOrderService orders, ILogger<OrdersHttpFunction> logger)
    {
        _orders = orders;
        _logger = logger;
    }

    // GET /api/orders/{orderId}
    [Function("GetOrder")]
    public IActionResult GetOrder(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "orders/{orderId}")] HttpRequest req,
        string orderId)
    {
        _logger.LogInformation("Fetching order {OrderId}", orderId);

        var order = _orders.GetById(orderId);
        if (order == null)
            return new NotFoundObjectResult(new { error = $"Order '{orderId}' not found." });

        return new OkObjectResult(order);
    }

    // POST /api/orders
    // Uses MultiOutput to simultaneously return 201 Created AND enqueue the order.
    [Function("PlaceOrder")]
    public PlaceOrderOutput PlaceOrder(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders")] HttpRequest req)
    {
        using var reader = new StreamReader(req.Body);
        var body = reader.ReadToEnd();

        Order? order;
        try
        {
            order = JsonSerializer.Deserialize<Order>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return new PlaceOrderOutput
            {
                HttpResponse = new BadRequestObjectResult(new { error = "Invalid JSON body." })
            };
        }

        if (order == null || string.IsNullOrWhiteSpace(order.CustomerEmail) || order.Items.Count == 0)
        {
            return new PlaceOrderOutput
            {
                HttpResponse = new BadRequestObjectResult(new { error = "CustomerEmail and at least one Item are required." })
            };
        }

        order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);
        order.Status = OrderStatus.Pending;
        order.PlacedAt = DateTime.UtcNow;

        _orders.Create(order);
        _logger.LogInformation("Order {OrderId} placed by {Email} — £{Total:F2}", order.OrderId, order.CustomerEmail, order.TotalAmount);

        return new PlaceOrderOutput
        {
            // Queue message triggers OrderQueueFunction for async processing
            QueueMessage = JsonSerializer.Serialize(order),
            HttpResponse = new ObjectResult(new { orderId = order.OrderId, status = "Pending", total = order.TotalAmount })
            {
                StatusCode = StatusCodes.Status201Created
            }
        };
    }
}

// MultiOutput: one return type carries both the HTTP response and the queue output binding.
public class PlaceOrderOutput
{
    [QueueOutput("orders-queue", Connection = "AzureWebJobsStorage")]
    public string? QueueMessage { get; set; }

    [HttpResult]
    public IActionResult? HttpResponse { get; set; }
}
