using FirmSheetManager.Models;

namespace FirmSheetManager.Services;

/// <summary>
/// Main service for managing firm sheets and their associated operations
/// </summary>
public class FirmSheetService
{
    private readonly BarcodeService _barcodeService;
    private readonly ScanEventService _scanEventService;
    private readonly Dictionary<string, FirmSheet> _firmSheets;
    
    public FirmSheetService()
    {
        _barcodeService = new BarcodeService();
        _scanEventService = new ScanEventService();
        _firmSheets = new Dictionary<string, FirmSheet>();
    }
    
    /// <summary>
    /// Creates a new firm sheet
    /// </summary>
    public FirmSheet CreateFirmSheet(FirmSheetType type, DeliveryUnit deliveryUnit, string createdBy)
    {
        var firmSheetId = GenerateFirmSheetId(type, deliveryUnit);
        var firmSheet = new FirmSheet(firmSheetId, type, deliveryUnit, createdBy);
        
        _firmSheets[firmSheetId] = firmSheet;
        return firmSheet;
    }
    
    /// <summary>
    /// Finds a firm sheet by scanning a barcode
    /// </summary>
    public FirmSheet? FindFirmSheetByBarcode(string barcode)
    {
        if (!_barcodeService.IsValidBarcode(barcode))
            return null;
            
        var trackingNumber = _barcodeService.ExtractTrackingNumber(barcode);
        
        // Look for existing firm sheet containing this tracking number
        return _firmSheets.Values.FirstOrDefault(fs => 
            fs.MailPieces.Any(mp => mp.TrackingNumber == trackingNumber));
    }
    
    /// <summary>
    /// Processes a scanned barcode and generates scan events
    /// </summary>
    public FirmSheetProcessResult ProcessBarcodeScan(string barcode)
    {
        try
        {
            if (!_barcodeService.IsValidBarcode(barcode))
            {
                return new FirmSheetProcessResult
                {
                    Success = false,
                    ErrorMessage = "Invalid barcode format"
                };
            }
            
            var firmSheet = FindFirmSheetByBarcode(barcode);
            if (firmSheet == null)
            {
                return new FirmSheetProcessResult
                {
                    Success = false,
                    ErrorMessage = "No firm sheet found for this barcode"
                };
            }
            
            // Generate scan events for all pieces on the firm sheet
            var scanEvents = _scanEventService.GenerateAllScanEvents(firmSheet);
            firmSheet.IsProcessed = true;
            
            return new FirmSheetProcessResult
            {
                Success = true,
                FirmSheet = firmSheet,
                GeneratedScanEvents = scanEvents,
                Message = $"Generated {scanEvents.Count} scan events for {firmSheet.PieceCount} mail pieces"
            };
        }
        catch (Exception ex)
        {
            return new FirmSheetProcessResult
            {
                Success = false,
                ErrorMessage = $"Error processing barcode: {ex.Message}"
            };
        }
    }
    
    /// <summary>
    /// Adds a mail piece to an existing firm sheet
    /// </summary>
    public bool AddMailPieceToFirmSheet(string firmSheetId, string trackingNumber, string deliveryAddress)
    {
        if (_firmSheets.TryGetValue(firmSheetId, out var firmSheet))
        {
            var isReturnMerchandise = _barcodeService.IsReturnMerchandise(trackingNumber);
            var mailPiece = new MailPiece(trackingNumber, deliveryAddress, isReturnMerchandise);
            firmSheet.AddMailPiece(mailPiece);
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Gets all firm sheets
    /// </summary>
    public List<FirmSheet> GetAllFirmSheets()
    {
        return _firmSheets.Values.ToList();
    }
    
    /// <summary>
    /// Gets firm sheets by type
    /// </summary>
    public List<FirmSheet> GetFirmSheetsByType(FirmSheetType type)
    {
        return _firmSheets.Values.Where(fs => fs.Type == type).ToList();
    }
    
    private string GenerateFirmSheetId(FirmSheetType type, DeliveryUnit deliveryUnit)
    {
        var typePrefix = type == FirmSheetType.CarrierRoute ? "CR" : "RM";
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        return $"{typePrefix}-{deliveryUnit.UnitId}-{timestamp}";
    }
}

/// <summary>
/// Result of processing a barcode scan
/// </summary>
public class FirmSheetProcessResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public FirmSheet? FirmSheet { get; set; }
    public List<ScanEvent> GeneratedScanEvents { get; set; } = new();
}