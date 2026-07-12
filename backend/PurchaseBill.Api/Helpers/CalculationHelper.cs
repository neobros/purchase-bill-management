namespace PurchaseBill.Api.Helpers;

/// <summary>
/// Pure, side-effect-free bill line calculations, kept out of the service
/// layer so the formulas can be unit tested in isolation.
/// </summary>
public static class CalculationHelper
{
    /// <summary>
    /// Total Cost = (Standard Cost * Qty) - Discount% of that base.
    /// Total Selling = Standard Price * Qty (discount does not apply to selling price).
    /// </summary>
    public static (decimal TotalCost, decimal TotalSelling) CalculateLineTotals(
        decimal standardCost, decimal standardPrice, int qty, decimal discountPercent)
    {
        var costBase = standardCost * qty;
        var totalCost = costBase - (costBase * discountPercent / 100m);
        var totalSelling = standardPrice * qty;

        return (totalCost, totalSelling);
    }
}
