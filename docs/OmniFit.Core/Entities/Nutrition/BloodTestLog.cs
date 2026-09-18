using System;

namespace OmniFit.Core.Entities.Nutrition;

public class BloodTestLog
{
    public Guid Id { get; set; }
    public Guid TraineeUserId { get; set; }
    public DateTime TestDate { get; set; }
    public string NutrientName { get; set; } = string.Empty;
    public decimal MeasuredValue { get; set; }
    public string DeficiencyStatus { get; set; } = string.Empty;
}
