namespace CarbonKitchen.Ingredients.Api.Validators
{
    using CarbonKitchen.Ingredients.Api.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class IngredientForCreationDtoValidator : IngredientForManipulationDtoValidator<IngredientForCreationDto>
    {
        public IngredientForCreationDtoValidator()
        {
        }
    }
}
