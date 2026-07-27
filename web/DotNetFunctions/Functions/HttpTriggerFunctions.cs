using DotNetFunctions.Models;
using DotNetFunctions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
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

public class HttpTriggerFunctions
{
    private readonly IOrderService _orders;
    private readonly ILogger<HttpTriggerFunctions> _logger;

    public HttpTriggerFunctions(IOrderService orders, ILogger<HttpTriggerFunctions> logger)
    {
        _orders = orders;
        _logger = logger;
    }

    // GET /api/orders/{orderId}
    [Function("GetOrder")]
    public async Task<HttpResponseData> GetOrder(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "orders/{orderId}")] HttpRequestData req,
        string orderId)
    {
        _logger.LogInformation("Fetching order {OrderId}", orderId);

        var order = _orders.GetById(orderId);

        if (order == null)
        {
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            await notFoundResponse.WriteAsJsonAsync(new { error = $"Order '{orderId}' not found." });
            return notFoundResponse;
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(order);
        return response;
    }

    [Function("CreateOrder")]
    public async Task<HttpResponseData> CreateOrder(
     [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders")] HttpRequestData req,
     FunctionContext context)
    {
        var logger = context.GetLogger("CreateOrder");

        // 1. Read query string params
        var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        string? source = query["source"]; // e.g. ?source=web

        // 2. Read a header
        string? correlationId = req.Headers.TryGetValues("X-Correlation-Id", out var values)
            ? values.FirstOrDefault()
            : Guid.NewGuid().ToString();

        logger.LogInformation("CreateOrder called | source={Source} | correlationId={CorrelationId}", source, correlationId);

        // 3. Read and validate JSON body
        Order? order;
        try
        {
            order = await req.ReadFromJsonAsync<Order>();
        }
        catch (JsonException)
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteAsJsonAsync(new { error = "Malformed JSON body." });
            return badRequest;
        }

        if (order is null || string.IsNullOrWhiteSpace(order.CustomerId))
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteAsJsonAsync(new { error = "CustomerId is required." });
            return badRequest;
        }

        // 4. "Business logic" (stand-in — swap for real save call)
        order.OrderId = Guid.NewGuid().ToString();
        _orders.Create(order);

        // 5. Build success response — status code, headers, and body all set
        var response = req.CreateResponse(HttpStatusCode.Created);
        response.Headers.Add("Location", $"/api/orders/{order.OrderId}");
        response.Headers.Add("X-Correlation-Id", correlationId!);
        await response.WriteAsJsonAsync(order);

        return response;
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
