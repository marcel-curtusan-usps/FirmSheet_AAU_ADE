using FirmSheetManager.Models;

namespace FirmSheetManager.Services;

/// <summary>
/// Service for generating and managing scan events
/// </summary>
public class ScanEventService
{
    /// <summary>
    /// Generates AAU (Arrival at Unit) scan events for all pieces on a firm sheet
    /// </summary>
    public List<ScanEvent> GenerateAAUScans(FirmSheet firmSheet)
    {
        var scanEvents = new List<ScanEvent>();
        
        foreach (var mailPiece in firmSheet.MailPieces)
        {
            var scanEvent = new ScanEvent(
                mailPiece.TrackingNumber,
                ScanEventType.AAU,
                firmSheet.DeliveryUnit.UnitId,
                $"{firmSheet.DeliveryUnit.City}, {firmSheet.DeliveryUnit.State}"
            );
            
            scanEvents.Add(scanEvent);
        }
        
        return scanEvents;
    }
    
    /// <summary>
    /// Generates ADE (Acceptable Delivery Event) scan events for all pieces on a firm sheet
    /// </summary>
    public List<ScanEvent> GenerateADEScans(FirmSheet firmSheet)
    {
        var scanEvents = new List<ScanEvent>();
        
        foreach (var mailPiece in firmSheet.MailPieces)
        {
            var scanEvent = new ScanEvent(
                mailPiece.TrackingNumber,
                ScanEventType.ADE,
                firmSheet.DeliveryUnit.UnitId,
                $"{firmSheet.DeliveryUnit.City}, {firmSheet.DeliveryUnit.State}"
            );
            
            scanEvents.Add(scanEvent);
        }
        
        return scanEvents;
    }
    
    /// <summary>
    /// Generates both AAU and ADE scan events for a firm sheet
    /// </summary>
    public List<ScanEvent> GenerateAllScanEvents(FirmSheet firmSheet)
    {
        var allScans = new List<ScanEvent>();
        
        // Generate AAU scans first (arrival at unit happens before delivery)
        allScans.AddRange(GenerateAAUScans(firmSheet));
        
        // Add a small delay for ADE scans to show proper sequence
        var adeScans = GenerateADEScans(firmSheet);
        foreach (var adeScan in adeScans)
        {
            adeScan.Timestamp = adeScan.Timestamp.AddMinutes(30); // ADE typically happens 30 mins after AAU
        }
        allScans.AddRange(adeScans);
        
        return allScans;
    }
    
    /// <summary>
    /// Formats scan events for display or export
    /// </summary>
    public string FormatScanEvents(List<ScanEvent> scanEvents)
    {
        var output = new System.Text.StringBuilder();
        output.AppendLine("Generated Scan Events:");
        output.AppendLine(new string('-', 80));
        
        foreach (var scanEvent in scanEvents.OrderBy(s => s.Timestamp))
        {
            output.AppendLine($"{scanEvent.Timestamp:yyyy-MM-dd HH:mm:ss} | " +
                             $"{scanEvent.EventType} | " +
                             $"{scanEvent.TrackingNumber} | " +
                             $"{scanEvent.Location} | " +
                             $"{scanEvent.EventDescription}");
        }
        
        return output.ToString();
    }
}