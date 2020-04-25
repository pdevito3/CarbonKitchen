﻿namespace CarbonKitchen.Ingredients.Api.Tests.RepositoryTests
{
    using FluentAssertions;
    using CarbonKitchen.Ingredients.Api.Data;
    using CarbonKitchen.Ingredients.Api.Services;
    using CarbonKitchen.Ingredients.Api.Tests.Fakes;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class DeleteIngredientRepositoryTests
    {
        [Fact]
        public void DeleteIngredient_ReturnsProperCount()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<IngredientDbContext>()
                .UseInMemoryDatabase(databaseName: $"IngredientDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeIngredientOne = new FakeIngredient { }.Generate();
            var fakeIngredientTwo = new FakeIngredient { }.Generate();
            var fakeIngredientThree = new FakeIngredient { }.Generate();

            //Act
            using (var context = new IngredientDbContext(dbOptions))
            {
                context.Ingredients.AddRange(fakeIngredientOne, fakeIngredientTwo, fakeIngredientThree);

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteIngredient(fakeIngredientTwo);

                context.SaveChanges();

                //Assert
                var ingredientList = context.Ingredients.ToList();

                ingredientList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                ingredientList.Should().ContainEquivalentOf(fakeIngredientOne);
                ingredientList.Should().ContainEquivalentOf(fakeIngredientThree);
                Assert.DoesNotContain(ingredientList, i => i == fakeIngredientTwo);

                context.Database.EnsureDeleted();
            }
        }
    }
}
