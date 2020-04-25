namespace CarbonKitchen.Ingredients.Api.Models
{
    using System;

    public class IngredientForManipulationDto
    {
        public int? RecipeId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double? Amount { get; set; }
    }
}
