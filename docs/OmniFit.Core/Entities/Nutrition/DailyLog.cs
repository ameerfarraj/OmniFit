using System;
using System.Collections.Generic;

namespace OmniFit.Core.Entities.Nutrition;

public class DailyLog
{
    public Guid Id { get; set; }
    public Guid TraineeUserId { get; set; }
    public DateOnly Date { get; set; }
    public bool IsTrainingDay { get; set; }
    public bool IsCheatMeal { get; set; }
    public int TotalCaloriesConsumed { get; set; }

    public ICollection<MealLogItem> Meals { get; set; } = new List<MealLogItem>();
}
