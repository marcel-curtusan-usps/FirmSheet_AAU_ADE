# FirmSheet_AAU_ADE

Create Firm sheets for Carrier Routes and Return Merchandise. Scan a single barcode and generate the Arrival at Unit and Acceptable Delivery Event (ADE) scans for the Delivery Unit for all pieces associated with that firm sheet.

## Features

- **Firm Sheet Management**: Create and manage firm sheets for Carrier Routes and Return Merchandise
- **Barcode Scanning**: Scan any barcode from a firm sheet to trigger scan event generation
- **Automatic Scan Generation**: Generate both AAU (Arrival at Unit) and ADE (Acceptable Delivery Event) scans for all mail pieces on the firm sheet
- **Delivery Unit Support**: Target specific delivery units for scan events
- **Interactive CLI**: User-friendly command-line interface for all operations

## Getting Started

### Prerequisites
- .NET 8.0 or later

### Running the Application

1. Navigate to the FirmSheetManager directory:
   ```bash
   cd FirmSheetManager
   ```

2. Build the application:
   ```bash
   dotnet build
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

### Usage

The application starts with sample data containing:
- 1 Carrier Route firm sheet with 3 mail pieces
- 1 Return Merchandise firm sheet with 2 mail pieces

#### Main Functions

1. **Scan Barcode**: Enter any tracking number from the sample data to generate AAU/ADE events
   - Sample Carrier Route tracking: `9400111202555003456787`
   - Sample Return Merchandise tracking: `RM9400111202555003456790`

2. **Create New Firm Sheet**: Set up new firm sheets for different delivery units

3. **View All Firm Sheets**: Display all firm sheets and their status

4. **Add Mail Pieces**: Add new mail pieces to existing firm sheets

#### Sample Output

When scanning a barcode, the system generates:
- **AAU (Arrival at Unit)** scans for all pieces on the firm sheet
- **ADE (Acceptable Delivery Event)** scans 30 minutes later for all pieces
- Detailed event log with timestamps, tracking numbers, and locations

```
✓ Generated 6 scan events for 3 mail pieces
Firm Sheet: CR-DU001-20250917215643
Type: CarrierRoute
Delivery Unit: Downtown Station

Generated Scan Events:
--------------------------------------------------------------------------------
2025-09-17 21:56:43 | AAU | 9400111202555003456787 | New York, NY | Arrival at Unit
2025-09-17 21:56:43 | AAU | 9400111202555003456788 | New York, NY | Arrival at Unit
2025-09-17 21:56:43 | AAU | 9400111202555003456789 | New York, NY | Arrival at Unit
2025-09-17 22:26:43 | ADE | 9400111202555003456787 | New York, NY | Acceptable Delivery Event
2025-09-17 22:26:43 | ADE | 9400111202555003456788 | New York, NY | Acceptable Delivery Event
2025-09-17 22:26:43 | ADE | 9400111202555003456789 | New York, NY | Acceptable Delivery Event
```

## Architecture

The application is structured with the following components:

- **Models**: Core data structures (FirmSheet, MailPiece, DeliveryUnit, ScanEvent)
- **Services**: Business logic (FirmSheetService, BarcodeService, ScanEventService)
- **Program**: Interactive CLI interface

## Supported Barcode Formats

- Standard USPS tracking numbers (20+ digits starting with 9)
- Return merchandise tracking (RM prefix + standard tracking)
- UPS-style tracking (1Z prefix + digits)
