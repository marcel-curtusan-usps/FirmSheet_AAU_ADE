namespace FirmSheetManager.Models;

/// <summary>
/// Represents a scan event for a mail piece
/// </summary>
public class ScanEvent
{
    public string TrackingNumber { get; set; } = string.Empty;
    public ScanEventType EventType { get; set; }
    public DateTime Timestamp { get; set; }
    public string DeliveryUnitId { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string EventDescription { get; set; } = string.Empty;
    
    public ScanEvent() { }
    
    public ScanEvent(string trackingNumber, ScanEventType eventType, string deliveryUnitId, string location)
    {
        TrackingNumber = trackingNumber;
        EventType = eventType;
        DeliveryUnitId = deliveryUnitId;
        Location = location;
        Timestamp = DateTime.UtcNow;
        EventDescription = eventType switch
        {
            ScanEventType.AAU => "Arrival at Unit",
            ScanEventType.ADE => "Acceptable Delivery Event",
            _ => "Unknown Event"
        };
    }
}