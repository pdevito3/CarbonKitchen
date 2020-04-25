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
                ingredientById.Ingredient.Should().Be(fakeIngredient.Ingredient);
                ingredientById.IngredientTextField2.Should().Be(fakeIngredient.IngredientTextField2);
                ingredientById.IngredientDateField1.Should().Be(fakeIngredient.IngredientDateField1);
            }
        }
    }
}
