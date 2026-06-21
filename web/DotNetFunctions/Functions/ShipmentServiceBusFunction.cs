using Azure.Messaging.ServiceBus;
using DotNetFunctions.Models;
using DotNetFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DotNetFunctions.Functions;

// ─── Service Bus Trigger ──────────────────────────────────────────────────────
// Use case: Real-time shipment tracking from a logistics partner.
//
// The logistics provider (DHL, FedEx, etc.) publishes shipment status events to
// an Azure Service Bus queue "shipment-updates". This function picks them up and
// updates the corresponding order status so customers see live tracking.
//
// Service Bus vs Storage Queue:
//   - Service Bus supports message sessions, dead-lettering, duplicate detection,
//     and ordering guarantees — important for transactional logistics events.
//   - Storage Queue is simpler and cheaper but lacks these guarantees.
// ─────────────────────────────────────────────────────────────────────────────

public class ShipmentServiceBusFunction
{
    private readonly IOrderService _orders;
    private readonly ILogger<ShipmentServiceBusFunction> _logger;

    public ShipmentServiceBusFunction(IOrderService orders, ILogger<ShipmentServiceBusFunction> logger)
    {
        _orders = orders;
        _logger = logger;
    }

    [Function("ProcessShipmentUpdate")]
    public async Task Run(
        [ServiceBusTrigger("shipment-updates", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext context)
    {
        _logger.LogInformation("Received shipment message {MessageId}", message.MessageId);

        ShipmentEvent? shipment;
        try
        {
            shipment = JsonSerializer.Deserialize<ShipmentEvent>(message.Body.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize shipment message {MessageId}", message.MessageId);
            // Dead-letter the message so it doesn't loop forever
            await messageActions.DeadLetterMessageAsync(message,
                deadLetterReason: "DeserializationFailed",
                deadLetterErrorDescription: ex.Message);
            return;
        }

        if (shipment == null || string.IsNullOrWhiteSpace(shipment.OrderId))
        {
            await messageActions.DeadLetterMessageAsync(message,
                deadLetterReason: "InvalidPayload",
                deadLetterErrorDescription: "Missing OrderId");
            return;
        }

        _logger.LogInformation(
            "Shipment update for order {OrderId}: {Status} via {Carrier} — tracking {Tracking} @ {Location}",
            shipment.OrderId, shipment.Status, shipment.Carrier, shipment.TrackingNumber, shipment.Location);

        var orderStatus = shipment.Status switch
        {
            ShipmentStatus.Dispatched or ShipmentStatus.InTransit or ShipmentStatus.OutForDelivery
                => OrderStatus.Shipped,
            ShipmentStatus.Delivered => OrderStatus.Delivered,
            ShipmentStatus.Failed => OrderStatus.Cancelled,
            _ => OrderStatus.Processing
        };

        var updated = _orders.UpdateStatus(shipment.OrderId, orderStatus);
        if (!updated)
            _logger.LogWarning("Order {OrderId} not found in store — shipment event may be stale", shipment.OrderId);

        // Complete the message to remove it from the queue
        await messageActions.CompleteMessageAsync(message);
    }
}
