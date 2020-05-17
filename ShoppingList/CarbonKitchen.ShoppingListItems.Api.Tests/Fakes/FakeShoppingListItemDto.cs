﻿using AutoBogus;
using CarbonKitchen.ShoppingListItems.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonKitchen.ShoppingListItems.Api.Tests.Fakes
{
    // or replace 'AutoFaker' with 'Faker' if you don't want all fields to be auto faked
    public class FakeShoppingListItemDto : AutoFaker<ShoppingListItemDto>
    {
        public FakeShoppingListItemDto()
        {
            // leaving the first 49 for potential special use cases in startup builds that need explicit values
            RuleFor(sli => sli.ShoppingListItemId, sli => sli.Random.Number(50, 100000));
        }
    }
}
