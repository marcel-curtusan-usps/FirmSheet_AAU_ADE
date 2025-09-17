namespace FirmSheetManager.Models;

/// <summary>
/// Represents a mail piece with tracking information
/// </summary>
public class MailPiece
{
    public string TrackingNumber { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public DateTime ProcessedDate { get; set; }
    public bool IsReturnMerchandise { get; set; }
    
    public MailPiece() { }
    
    public MailPiece(string trackingNumber, string deliveryAddress, bool isReturnMerchandise = false)
    {
        TrackingNumber = trackingNumber;
        DeliveryAddress = deliveryAddress;
        IsReturnMerchandise = isReturnMerchandise;
        ProcessedDate = DateTime.UtcNow;
    }
}