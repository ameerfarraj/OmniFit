using System;

namespace OmniFit.Core.Entities.Nutrition;

public class RecipeIngredient
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public Guid FoodItemId { get; set; }
    public decimal AmountGrams { get; set; }
}
