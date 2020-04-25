namespace CarbonKitchen.Ingredients.Api.Validators
{
    using FluentValidation;
    using CarbonKitchen.Ingredients.Api.Models;
    using System;

    public class IngredientForManipulationDtoValidator<T> : AbstractValidator<T> where T : IngredientForManipulationDto
    {
        public IngredientForManipulationDtoValidator()
        {
            //RuleFor(i => i.Ingredient);
            RuleFor(i => i.RecipeId)
                .GreaterThanOrEqualTo(0);
            RuleFor(i => i.Amount)
                .GreaterThanOrEqualTo(0);
        }
    }
}
