namespace CarbonKitchen.Ingredients.Api.Tests.RepositoryTests
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
    using System.Linq;
    using Xunit;
    using System.Collections.Generic;
    using CarbonKitchen.Ingredients.Api.Data.Entities;

    public class CreateIngredientRepositoryTests
    {
        [Fact]
        public void AddIngredient_NewRecordAddedWithProperValues()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<IngredientDbContext>()
                .UseInMemoryDatabase(databaseName: $"IngredientDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeIngredient = new FakeIngredient { }.Generate();

            //Act
            using (var context = new IngredientDbContext(dbOptions))
            {
                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                service.AddIngredient(fakeIngredient);

                context.SaveChanges();
            }

            //Assert
            using (var context = new IngredientDbContext(dbOptions))
            {
                context.Ingredients.Count().Should().Be(1);

                var ingredientById = context.Ingredients.FirstOrDefault(i => i.IngredientId == fakeIngredient.IngredientId);

                ingredientById.Should().BeEquivalentTo(fakeIngredient);
                ingredientById.IngredientId.Should().Be(fakeIngredient.IngredientId);
                ingredientById.Name.Should().Be(fakeIngredient.Name);
                ingredientById.Unit.Should().Be(fakeIngredient.Unit);
                ingredientById.Amount.Should().Be(fakeIngredient.Amount);
            }
        }

        [Fact]
        public void AddIngredients_NewRecordAddedWithProperValues()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<IngredientDbContext>()
                .UseInMemoryDatabase(databaseName: $"IngredientDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeIngredientOne = new FakeIngredient { }.Generate();
            var fakeIngredientTwo = new FakeIngredient { }.Generate();
            var fakeIngredientThree = new FakeIngredient { }.Generate();
            var iList = new List<Ingredient>() 
            {
                fakeIngredientOne
                , fakeIngredientTwo
                , fakeIngredientThree
            };

            //Act
            using (var context = new IngredientDbContext(dbOptions))
            {
                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                service.AddIngredients(iList);

                context.SaveChanges();
            }

            //Assert
            using (var context = new IngredientDbContext(dbOptions))
            {
                context.Ingredients.Count().Should().Be(3);

                var ingredients = context.Ingredients.ToList();

                ingredients.Should().ContainEquivalentOf(fakeIngredientOne);
                ingredients.Should().ContainEquivalentOf(fakeIngredientTwo);
                ingredients.Should().ContainEquivalentOf(fakeIngredientThree);
            }
        }
    }
}
