namespace PurchaseBill.Api.DTOs;

public class PurchaseBillResponseDto
{
    public int Id { get; set; }
    public DateTime BillDate { get; set; }
    public List<PurchaseBillItemResponseDto> Items { get; set; } = new();

    public int TotalItems => Items.Count;
    public int TotalQty => Items.Sum(i => i.Qty);
    public decimal GrossTotal => Items.Sum(i => i.TotalSelling);
    public decimal TotalCost => Items.Sum(i => i.TotalCost);
}
