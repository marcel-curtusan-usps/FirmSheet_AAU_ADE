using FirmSheetManager.Models;
using FirmSheetManager.Services;

namespace FirmSheetManager;

class Program
{
    private static readonly FirmSheetService _firmSheetService = new();
    private static readonly ScanEventService _scanEventService = new();
    
    static void Main(string[] args)
    {
        // Check if running tests
        if (args.Length > 0 && args[0] == "--test")
        {
            FirmSheetManager.Tests.BasicFunctionalityTests.RunAllTests();
            return;
        }
        
        Console.WriteLine("=================================================");
        Console.WriteLine("USPS Firm Sheet Manager - AAU/ADE Scan Generator");
        Console.WriteLine("=================================================");
        Console.WriteLine();
        
        // Initialize with some sample data for demonstration
        InitializeSampleData();
        
        bool running = true;
        while (running)
        {
            ShowMainMenu();
            var choice = Console.ReadLine()?.Trim();
            
            switch (choice)
            {
                case "1":
                    ScanBarcode();
                    break;
                case "2":
                    CreateNewFirmSheet();
                    break;
                case "3":
                    ViewFirmSheets();
                    break;
                case "4":
                    AddMailPieceToFirmSheet();
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
            
            if (running)
            {
                Console.WriteLine("\nPress any key to continue...");
                try
                {
                    Console.ReadKey();
                }
                catch (InvalidOperationException)
                {
                    // Handle case where console input is redirected (for testing)
                    System.Threading.Thread.Sleep(100);
                }
                Console.Clear();
            }
        }
        
        Console.WriteLine("Thank you for using USPS Firm Sheet Manager!");
    }
    
    private static void ShowMainMenu()
    {
        Console.WriteLine("Main Menu:");
        Console.WriteLine("1. Scan Barcode (Generate AAU/ADE Events)");
        Console.WriteLine("2. Create New Firm Sheet");
        Console.WriteLine("3. View All Firm Sheets");
        Console.WriteLine("4. Add Mail Piece to Firm Sheet");
        Console.WriteLine("5. Exit");
        Console.WriteLine();
        Console.Write("Select an option (1-5): ");
    }
    
    private static void ScanBarcode()
    {
        Console.WriteLine("\n--- Barcode Scanning ---");
        Console.Write("Enter barcode: ");
        var barcode = Console.ReadLine()?.Trim();
        
        if (string.IsNullOrWhiteSpace(barcode))
        {
            Console.WriteLine("No barcode entered.");
            return;
        }
        
        var result = _firmSheetService.ProcessBarcodeScan(barcode);
        
        if (result.Success)
        {
            Console.WriteLine($"\n✓ {result.Message}");
            Console.WriteLine($"Firm Sheet: {result.FirmSheet?.FirmSheetId}");
            Console.WriteLine($"Type: {result.FirmSheet?.Type}");
            Console.WriteLine($"Delivery Unit: {result.FirmSheet?.DeliveryUnit.UnitName}");
            Console.WriteLine();
            
            var formattedEvents = _scanEventService.FormatScanEvents(result.GeneratedScanEvents);
            Console.WriteLine(formattedEvents);
        }
        else
        {
            Console.WriteLine($"\n✗ Error: {result.ErrorMessage}");
        }
    }
    
    private static void CreateNewFirmSheet()
    {
        Console.WriteLine("\n--- Create New Firm Sheet ---");
        
        Console.WriteLine("Select firm sheet type:");
        Console.WriteLine("1. Carrier Route");
        Console.WriteLine("2. Return Merchandise");
        Console.Write("Enter choice (1-2): ");
        
        var typeChoice = Console.ReadLine()?.Trim();
        FirmSheetType type = typeChoice == "2" ? FirmSheetType.ReturnMerchandise : FirmSheetType.CarrierRoute;
        
        Console.Write("Enter delivery unit ID: ");
        var unitId = Console.ReadLine()?.Trim() ?? "";
        
        Console.Write("Enter delivery unit name: ");
        var unitName = Console.ReadLine()?.Trim() ?? "";
        
        Console.Write("Enter ZIP code: ");
        var zipCode = Console.ReadLine()?.Trim() ?? "";
        
        Console.Write("Enter city: ");
        var city = Console.ReadLine()?.Trim() ?? "";
        
        Console.Write("Enter state: ");
        var state = Console.ReadLine()?.Trim() ?? "";
        
        Console.Write("Enter your name: ");
        var createdBy = Console.ReadLine()?.Trim() ?? "Unknown";
        
        var deliveryUnit = new DeliveryUnit(unitId, unitName, zipCode, city, state);
        var firmSheet = _firmSheetService.CreateFirmSheet(type, deliveryUnit, createdBy);
        
        Console.WriteLine($"\n✓ Created firm sheet: {firmSheet.FirmSheetId}");
    }
    
    private static void ViewFirmSheets()
    {
        Console.WriteLine("\n--- All Firm Sheets ---");
        
        var firmSheets = _firmSheetService.GetAllFirmSheets();
        
        if (firmSheets.Count == 0)
        {
            Console.WriteLine("No firm sheets found.");
            return;
        }
        
        foreach (var fs in firmSheets)
        {
            Console.WriteLine($"ID: {fs.FirmSheetId}");
            Console.WriteLine($"Type: {fs.Type}");
            Console.WriteLine($"Delivery Unit: {fs.DeliveryUnit.UnitName} ({fs.DeliveryUnit.UnitId})");
            Console.WriteLine($"Mail Pieces: {fs.PieceCount}");
            Console.WriteLine($"Created: {fs.CreatedDate:yyyy-MM-dd HH:mm:ss} by {fs.CreatedBy}");
            Console.WriteLine($"Processed: {(fs.IsProcessed ? "Yes" : "No")}");
            Console.WriteLine(new string('-', 50));
        }
    }
    
    private static void AddMailPieceToFirmSheet()
    {
        Console.WriteLine("\n--- Add Mail Piece to Firm Sheet ---");
        
        var firmSheets = _firmSheetService.GetAllFirmSheets();
        if (firmSheets.Count == 0)
        {
            Console.WriteLine("No firm sheets available. Create one first.");
            return;
        }
        
        Console.WriteLine("Available firm sheets:");
        for (int i = 0; i < firmSheets.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {firmSheets[i].FirmSheetId} - {firmSheets[i].Type}");
        }
        
        Console.Write("Select firm sheet (enter number): ");
        if (int.TryParse(Console.ReadLine(), out int selection) && 
            selection > 0 && selection <= firmSheets.Count)
        {
            var selectedFirmSheet = firmSheets[selection - 1];
            
            Console.Write("Enter tracking number: ");
            var trackingNumber = Console.ReadLine()?.Trim() ?? "";
            
            Console.Write("Enter delivery address: ");
            var deliveryAddress = Console.ReadLine()?.Trim() ?? "";
            
            if (_firmSheetService.AddMailPieceToFirmSheet(selectedFirmSheet.FirmSheetId, trackingNumber, deliveryAddress))
            {
                Console.WriteLine("✓ Mail piece added successfully.");
            }
            else
            {
                Console.WriteLine("✗ Failed to add mail piece.");
            }
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }
    
    private static void InitializeSampleData()
    {
        // Create sample delivery units and firm sheets for testing
        var deliveryUnit1 = new DeliveryUnit("DU001", "Downtown Station", "10001", "New York", "NY");
        var deliveryUnit2 = new DeliveryUnit("DU002", "Suburban Branch", "90210", "Beverly Hills", "CA");
        
        var carrierRouteSheet = _firmSheetService.CreateFirmSheet(FirmSheetType.CarrierRoute, deliveryUnit1, "System");
        var returnMerchandiseSheet = _firmSheetService.CreateFirmSheet(FirmSheetType.ReturnMerchandise, deliveryUnit2, "System");
        
        // Add sample mail pieces
        _firmSheetService.AddMailPieceToFirmSheet(carrierRouteSheet.FirmSheetId, "9400111202555003456787", "123 Main St, New York, NY 10001");
        _firmSheetService.AddMailPieceToFirmSheet(carrierRouteSheet.FirmSheetId, "9400111202555003456788", "456 Oak Ave, New York, NY 10001");
        _firmSheetService.AddMailPieceToFirmSheet(carrierRouteSheet.FirmSheetId, "9400111202555003456789", "789 Pine St, New York, NY 10001");
        
        _firmSheetService.AddMailPieceToFirmSheet(returnMerchandiseSheet.FirmSheetId, "RM9400111202555003456790", "321 Elm St, Beverly Hills, CA 90210");
        _firmSheetService.AddMailPieceToFirmSheet(returnMerchandiseSheet.FirmSheetId, "RM9400111202555003456791", "654 Maple Dr, Beverly Hills, CA 90210");
        
        Console.WriteLine("Sample data initialized with 2 firm sheets and 5 mail pieces.");
        Console.WriteLine("You can scan any of the tracking numbers to generate AAU/ADE events.");
        Console.WriteLine();
    }
}
