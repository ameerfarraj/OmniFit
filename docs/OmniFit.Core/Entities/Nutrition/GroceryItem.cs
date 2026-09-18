using System;

namespace OmniFit.Core.Entities.Nutrition;

public class GroceryItem
{
    public Guid Id { get; set; }
    public Guid NutritionPlanId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public bool IsPurchased { get; set; }
}
