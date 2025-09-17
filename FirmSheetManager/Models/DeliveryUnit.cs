namespace FirmSheetManager.Models;

/// <summary>
/// Represents a postal delivery unit
/// </summary>
public class DeliveryUnit
{
    public string UnitId { get; set; } = string.Empty;
    public string UnitName { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    
    public DeliveryUnit() { }
    
    public DeliveryUnit(string unitId, string unitName, string zipCode, string city, string state)
    {
        UnitId = unitId;
        UnitName = unitName;
        ZipCode = zipCode;
        City = city;
        State = state;
    }
}