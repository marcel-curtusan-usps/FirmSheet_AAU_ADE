namespace FirmSheetManager.Models;

/// <summary>
/// Represents a firm sheet containing mail pieces for a specific delivery unit
/// </summary>
public class FirmSheet
{
    public string FirmSheetId { get; set; } = string.Empty;
    public FirmSheetType Type { get; set; }
    public DeliveryUnit DeliveryUnit { get; set; } = new();
    public List<MailPiece> MailPieces { get; set; } = new();
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public bool IsProcessed { get; set; }
    
    public FirmSheet() { }
    
    public FirmSheet(string firmSheetId, FirmSheetType type, DeliveryUnit deliveryUnit, string createdBy)
    {
        FirmSheetId = firmSheetId;
        Type = type;
        DeliveryUnit = deliveryUnit;
        CreatedBy = createdBy;
        CreatedDate = DateTime.UtcNow;
        IsProcessed = false;
    }
    
    /// <summary>
    /// Adds a mail piece to the firm sheet
    /// </summary>
    public void AddMailPiece(MailPiece mailPiece)
    {
        if (Type == FirmSheetType.ReturnMerchandise)
        {
            mailPiece.IsReturnMerchandise = true;
        }
        MailPieces.Add(mailPiece);
    }
    
    /// <summary>
    /// Gets the count of mail pieces on this firm sheet
    /// </summary>
    public int PieceCount => MailPieces.Count;
}