namespace CarbonKitchen.Recipes.Api.Validators
{
    using CarbonKitchen.Recipes.Api.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RecipeForCreationDtoValidator : RecipeForManipulationDtoValidator<RecipeForCreationDto>
    {
        public RecipeForCreationDtoValidator()
        {
        }
    }
}
