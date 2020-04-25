using AutoBogus;
using CarbonKitchen.Recipes.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonKitchen.Recipes.Api.Tests.Fakes
{
    // or replace 'AutoFaker' with 'Faker' if you don't want all fields to be auto faked
    public class FakeRecipeForUpdateDto : AutoFaker<RecipeForUpdateDto>
    {
        public FakeRecipeForUpdateDto()
        {
        }
    }
}
