using System;

namespace OmniFit.Core.Entities.Nutrition;

public class MealLogItem
{
    public Guid Id { get; set; }
    public Guid DailyLogId { get; set; }
    
    public Guid? FoodItemId { get; set; }
    public Guid? RecipeId { get; set; }
    
    public string MealType { get; set; } = string.Empty;
    public decimal WeightGrams { get; set; }
}
