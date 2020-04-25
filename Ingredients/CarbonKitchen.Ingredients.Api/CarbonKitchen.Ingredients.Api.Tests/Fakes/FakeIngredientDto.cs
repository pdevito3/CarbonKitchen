using AutoBogus;
using CarbonKitchen.Ingredients.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonKitchen.Ingredients.Api.Tests.Fakes
{
    // or replace 'AutoFaker' with 'Faker' if you don't want all fields to be auto faked
    public class FakeIngredientDto : AutoFaker<IngredientDto>
    {
        public FakeIngredientDto()
        {
            // leaving the first 49 for potential special use cases in startup builds that need explicit values
            RuleFor(i => i.IngredientId, i => i.Random.Number(50, 100000));
        }
    }
}
