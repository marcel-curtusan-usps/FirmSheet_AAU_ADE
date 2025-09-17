using FirmSheetManager.Models;

namespace FirmSheetManager.Services;

/// <summary>
/// Service for managing barcode scanning and validation
/// </summary>
public class BarcodeService
{
    /// <summary>
    /// Validates if a barcode is in the correct format
    /// </summary>
    public bool IsValidBarcode(string barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            return false;
            
        // Basic validation - typically USPS tracking numbers are 20-22 digits
        // but can also include some letter prefixes
        var cleanBarcode = barcode.Trim().ToUpper();
        
        // Check for common USPS tracking formats
        if (cleanBarcode.Length >= 20 && cleanBarcode.Length <= 35)
        {
            // Handle return merchandise prefix
            if (cleanBarcode.StartsWith("RM"))
            {
                var withoutRM = cleanBarcode.Substring(2);
                return withoutRM.Length >= 20 && withoutRM.All(char.IsDigit);
            }
            
            // Handle standard tracking numbers
            if (cleanBarcode.StartsWith("9") && cleanBarcode.Length >= 20 && cleanBarcode.All(char.IsDigit))
            {
                return true;
            }
            
            // Handle other prefixes like 1Z
            var withoutPrefixes = cleanBarcode.Replace("1Z", "");
            if (withoutPrefixes.Length >= 20 && withoutPrefixes.All(char.IsDigit))
            {
                return true;
            }
        }
        
        return false;
    }
    
    /// <summary>
    /// Extracts tracking number from a scanned barcode
    /// </summary>
    public string ExtractTrackingNumber(string barcode)
    {
        if (!IsValidBarcode(barcode))
            throw new ArgumentException("Invalid barcode format", nameof(barcode));
            
        return barcode.Trim().ToUpper();
    }
    
    /// <summary>
    /// Determines if a barcode represents return merchandise based on patterns
    /// </summary>
    public bool IsReturnMerchandise(string barcode)
    {
        var cleanBarcode = barcode.Trim().ToUpper();
        
        // Common patterns for return merchandise
        // This is a simplified implementation - real systems would have more complex logic
        return cleanBarcode.Contains("RM") || 
               cleanBarcode.StartsWith("RS") ||
               cleanBarcode.Contains("RETURN");
    }
}