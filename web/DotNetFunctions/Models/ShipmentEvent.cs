namespace DotNetFunctions.Models;

public class ShipmentEvent
{
    public string OrderId { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public string Carrier { get; set; } = string.Empty;
    public ShipmentStatus Status { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime EventTime { get; set; } = DateTime.UtcNow;
}

public enum ShipmentStatus { Dispatched, InTransit, OutForDelivery, Delivered, Failed }
