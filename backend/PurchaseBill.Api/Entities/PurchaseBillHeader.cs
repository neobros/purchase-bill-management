namespace PurchaseBill.Api.Entities;

/// <summary>
/// Named "Header" (not "PurchaseBill") to avoid colliding with the
/// PurchaseBill.Api root namespace, which would force fully-qualified
/// references everywhere the type is used unqualified.
/// </summary>
public class PurchaseBillHeader
{
    public int Id { get; set; }
    public DateTime BillDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<PurchaseBillItem> Items { get; set; } = new List<PurchaseBillItem>();
}
