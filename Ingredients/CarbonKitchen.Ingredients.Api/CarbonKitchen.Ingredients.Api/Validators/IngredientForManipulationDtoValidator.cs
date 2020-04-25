namespace CarbonKitchen.Ingredients.Api.Validators
{
    using FluentValidation;
    using CarbonKitchen.Ingredients.Api.Models;
    using System;

    public class IngredientForManipulationDtoValidator<T> : AbstractValidator<T> where T : IngredientForManipulationDto
    {
        public IngredientForManipulationDtoValidator()
        {
            RuleFor(i => i.Ingredient)
                .NotEmpty();
            RuleFor(i => i.RecipeId)
                .GreaterThanOrEqualTo(0);
            RuleFor(i => i.IngredientDateField1)
                .LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}
