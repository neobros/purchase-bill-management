namespace PurchaseBill.Api.DTOs;

public class PurchaseBillItemResponseDto
{
    public int Id { get; set; }
    public string Item { get; set; } = string.Empty;
    public string Batch { get; set; } = string.Empty;
    public decimal StandardCost { get; set; }
    public decimal StandardPrice { get; set; }
    public decimal Margin { get; set; }
    public int Qty { get; set; }
    public int FreeQty { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalSelling { get; set; }
}
