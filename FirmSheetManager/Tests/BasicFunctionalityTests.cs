using FirmSheetManager.Models;
using FirmSheetManager.Services;

namespace FirmSheetManager.Tests;

/// <summary>
/// Basic tests for FirmSheet functionality
/// </summary>
public class BasicFunctionalityTests
{
    public static void RunAllTests()
    {
        Console.WriteLine("Running Basic Functionality Tests...\n");
        
        TestBarcodeValidation();
        TestFirmSheetCreation();
        TestScanEventGeneration();
        TestEndToEndWorkflow();
        
        Console.WriteLine("All tests completed successfully!");
    }
    
    private static void TestBarcodeValidation()
    {
        Console.WriteLine("1. Testing Barcode Validation...");
        var barcodeService = new BarcodeService();
        
        // Test valid barcodes
        var validBarcodes = new[]
        {
            "9400111202555003456787",
            "RM9400111202555003456790"
        };
        
        foreach (var barcode in validBarcodes)
        {
            if (!barcodeService.IsValidBarcode(barcode))
            {
                throw new Exception($"Valid barcode rejected: {barcode}");
            }
        }
        
        // Test invalid barcodes
        var invalidBarcodes = new[] { "", "123", "INVALID", "RM123" };
        
        foreach (var barcode in invalidBarcodes)
        {
            if (barcodeService.IsValidBarcode(barcode))
            {
                throw new Exception($"Invalid barcode accepted: {barcode}");
            }
        }
        
        Console.WriteLine("   ✓ Barcode validation works correctly");
    }
    
    private static void TestFirmSheetCreation()
    {
        Console.WriteLine("2. Testing Firm Sheet Creation...");
        var firmSheetService = new FirmSheetService();
        var deliveryUnit = new DeliveryUnit("TEST001", "Test Unit", "12345", "Test City", "TS");
        
        var carrierSheet = firmSheetService.CreateFirmSheet(FirmSheetType.CarrierRoute, deliveryUnit, "TestUser");
        var returnSheet = firmSheetService.CreateFirmSheet(FirmSheetType.ReturnMerchandise, deliveryUnit, "TestUser");
        
        if (carrierSheet.Type != FirmSheetType.CarrierRoute)
            throw new Exception("Carrier route sheet type incorrect");
            
        if (returnSheet.Type != FirmSheetType.ReturnMerchandise)
            throw new Exception("Return merchandise sheet type incorrect");
            
        if (!carrierSheet.FirmSheetId.StartsWith("CR-"))
            throw new Exception("Carrier route ID format incorrect");
            
        if (!returnSheet.FirmSheetId.StartsWith("RM-"))
            throw new Exception("Return merchandise ID format incorrect");
        
        Console.WriteLine("   ✓ Firm sheet creation works correctly");
    }
    
    private static void TestScanEventGeneration()
    {
        Console.WriteLine("3. Testing Scan Event Generation...");
        var scanEventService = new ScanEventService();
        var deliveryUnit = new DeliveryUnit("TEST001", "Test Unit", "12345", "Test City", "TS");
        var firmSheet = new FirmSheet("TEST-001", FirmSheetType.CarrierRoute, deliveryUnit, "TestUser");
        
        // Add test mail pieces
        firmSheet.AddMailPiece(new MailPiece("9400111202555003456787", "123 Test St"));
        firmSheet.AddMailPiece(new MailPiece("9400111202555003456788", "456 Test Ave"));
        
        var scanEvents = scanEventService.GenerateAllScanEvents(firmSheet);
        
        if (scanEvents.Count != 4) // 2 AAU + 2 ADE
            throw new Exception($"Expected 4 scan events, got {scanEvents.Count}");
            
        var aauEvents = scanEvents.Where(e => e.EventType == ScanEventType.AAU).ToList();
        var adeEvents = scanEvents.Where(e => e.EventType == ScanEventType.ADE).ToList();
        
        if (aauEvents.Count != 2 || adeEvents.Count != 2)
            throw new Exception("Incorrect AAU/ADE event counts");
        
        Console.WriteLine("   ✓ Scan event generation works correctly");
    }
    
    private static void TestEndToEndWorkflow()
    {
        Console.WriteLine("4. Testing End-to-End Workflow...");
        var firmSheetService = new FirmSheetService();
        var deliveryUnit = new DeliveryUnit("TEST001", "Test Unit", "12345", "Test City", "TS");
        
        // Create firm sheet and add mail piece
        var firmSheet = firmSheetService.CreateFirmSheet(FirmSheetType.CarrierRoute, deliveryUnit, "TestUser");
        firmSheetService.AddMailPieceToFirmSheet(firmSheet.FirmSheetId, "9400111202555003456787", "123 Test St");
        
        // Process barcode scan
        var result = firmSheetService.ProcessBarcodeScan("9400111202555003456787");
        
        if (!result.Success)
            throw new Exception($"Barcode processing failed: {result.ErrorMessage}");
            
        if (result.GeneratedScanEvents.Count != 2) // 1 AAU + 1 ADE
            throw new Exception($"Expected 2 scan events, got {result.GeneratedScanEvents.Count}");
            
        if (!result.FirmSheet!.IsProcessed)
            throw new Exception("Firm sheet should be marked as processed");
        
        Console.WriteLine("   ✓ End-to-end workflow works correctly");
    }
}