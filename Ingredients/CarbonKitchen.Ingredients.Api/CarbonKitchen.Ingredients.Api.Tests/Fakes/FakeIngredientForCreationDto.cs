using AutoBogus;
using CarbonKitchen.Ingredients.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonKitchen.Ingredients.Api.Tests.Fakes
{
    // or replace 'AutoFaker' with 'Faker' if you don't want all fields to be auto faked
    public class FakeIngredientForCreationDto : AutoFaker<IngredientForCreationDto>
    {
        public FakeIngredientForCreationDto()
        {
        }
    }
}
