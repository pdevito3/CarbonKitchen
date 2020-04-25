﻿namespace CarbonKitchen.Ingredients.Api.Models
{
    using System;

    public class IngredientForManipulationDto
    {
        public int? RecipeId { get; set; }
        public string Ingredient { get; set; }
        public string IngredientTextField2 { get; set; }
        public DateTime? IngredientDateField1 { get; set; }
    }
}
