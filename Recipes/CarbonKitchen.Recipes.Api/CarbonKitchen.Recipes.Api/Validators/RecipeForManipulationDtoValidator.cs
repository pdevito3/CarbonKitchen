namespace CarbonKitchen.Recipes.Api.Validators
{
    using FluentValidation;
    using CarbonKitchen.Recipes.Api.Models;
    using System;

    public class RecipeForManipulationDtoValidator<T> : AbstractValidator<T> where T : RecipeForManipulationDto
    {
        public RecipeForManipulationDtoValidator()
        {
        }
    }
}
