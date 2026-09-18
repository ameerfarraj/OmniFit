using System;

namespace OmniFit.Core.Entities.Nutrition;

public class FoodItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Calories100g { get; set; }
    public decimal Protein100g { get; set; }
    public decimal Carbs100g { get; set; }
    public decimal Fat100g { get; set; }
    public bool IsOCRScanned { get; set; }
}
