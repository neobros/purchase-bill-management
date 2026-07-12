using System.ComponentModel.DataAnnotations;

namespace PurchaseBill.Api.DTOs;

/// <summary>Request payload for a single line item. Totals are recalculated server-side.</summary>
public class PurchaseBillItemDto
{
    [Required(ErrorMessage = "Item is required.")]
    public string Item { get; set; } = string.Empty;

    [Required(ErrorMessage = "Batch is required.")]
    public string Batch { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Standard Cost cannot be negative.")]
    public decimal StandardCost { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Standard Price cannot be negative.")]
    public decimal StandardPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Margin cannot be negative.")]
    public decimal Margin { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Qty { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Free Qty cannot be negative.")]
    public int FreeQty { get; set; }

    [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
    public decimal Discount { get; set; }
}
