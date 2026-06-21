using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DotNetFunctions.Functions;

// ─── Event Grid Trigger ───────────────────────────────────────────────────────
// Use case: Automated low-stock alerts.
//
// When the inventory service detects a product is running low, it publishes a
// "ProductStockLow" event to Azure Event Grid. This function reacts to that event
// and triggers a restock purchase order (simulated here as a log message).
//
// Event Grid vs Service Bus:
//   - Event Grid is designed for reactive, event-driven architectures (1:N fan-out).
//   - Multiple subscribers can react to the same event independently
//     (e.g., this function alerts purchasing; another notifies the warehouse).
// ─────────────────────────────────────────────────────────────────────────────

public class LowStockEventGridFunction
{
    private readonly ILogger<LowStockEventGridFunction> _logger;

    public LowStockEventGridFunction(ILogger<LowStockEventGridFunction> logger)
    {
        _logger = logger;
    }

    [Function("HandleLowStockAlert")]
    public void Run(
        [EventGridTrigger] string eventData,
        FunctionContext context)
    {
        _logger.LogInformation("Event Grid trigger fired with payload: {Data}", eventData);

        LowStockEvent? evt;
        try
        {
            // Event Grid sends a JSON array; unwrap the first element.
            var doc = JsonDocument.Parse(eventData);
            var root = doc.RootElement.ValueKind == JsonValueKind.Array
                ? doc.RootElement[0]
                : doc.RootElement;

            var eventType = root.TryGetProperty("eventType", out var et) ? et.GetString() : null;

            if (eventType != "ProductStockLow")
            {
                _logger.LogInformation("Ignoring event type '{EventType}'", eventType);
                return;
            }

            var dataElement = root.GetProperty("data");
            evt = dataElement.Deserialize<LowStockEvent>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse Event Grid payload");
            return;
        }

        if (evt == null) return;

        _logger.LogWarning(
            "LOW STOCK ALERT: Product '{ProductName}' [{ProductId}] has only {CurrentStock} units left (threshold: {Threshold}).",
            evt.ProductName, evt.ProductId, evt.CurrentStock, evt.Threshold);

        // In production: raise a purchase order, notify the buying team via email/Teams
        var restockQty = Math.Max(evt.Threshold * 10, 500);
        _logger.LogInformation(
            "Auto-raising restock order for {Qty} units of '{ProductName}'", restockQty, evt.ProductName);
    }
}

public class LowStockEvent
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int Threshold { get; set; }
}
