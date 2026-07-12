using System.ComponentModel.DataAnnotations;

namespace PurchaseBill.Api.DTOs;

public class PurchaseBillCreateDto
{
    public DateTime BillDate { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "At least one item is required.")]
    [MinLength(1, ErrorMessage = "At least one item is required.")]
    public List<PurchaseBillItemDto> Items { get; set; } = new();
}
