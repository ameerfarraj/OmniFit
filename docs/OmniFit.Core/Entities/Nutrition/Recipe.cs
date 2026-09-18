using System;
using System.Collections.Generic;

namespace OmniFit.Core.Entities.Nutrition;

public class Recipe
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal YieldWeight { get; set; }
    public decimal RawToCookedRatio { get; set; }
    
    public ICollection<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
}
