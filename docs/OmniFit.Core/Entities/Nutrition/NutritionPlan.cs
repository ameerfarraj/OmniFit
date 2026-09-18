using System;

namespace OmniFit.Core.Entities.Nutrition;

public class NutritionPlan
{
    public Guid Id { get; set; }
    public Guid TraineeUserId { get; set; }
    
    public int TargetCalories { get; set; }
    public decimal TargetProtein { get; set; }
    public decimal TargetCarbs { get; set; }
    public decimal TargetFat { get; set; }
    
    public bool IsManualOverride { get; set; }
    
    public string GoalType { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}
