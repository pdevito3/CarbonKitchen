using AutoBogus;
using CarbonKitchen.Recipes.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonKitchen.Recipes.Api.Tests.Fakes
{
    // or replace 'AutoFaker' with 'Faker' if you don't want all fields to be auto faked
    public class FakeRecipe : AutoFaker<Recipe>
    {
        public FakeRecipe()
        {
            // leaving the first 49 for potential special use cases in startup builds that need explicit values
            RuleFor(r => r.RecipeId, r => r.Random.Number(50, 100000));
        }
    }
}
