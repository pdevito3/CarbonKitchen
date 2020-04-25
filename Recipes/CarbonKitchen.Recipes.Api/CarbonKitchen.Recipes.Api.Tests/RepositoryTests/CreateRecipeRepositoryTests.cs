namespace CarbonKitchen.Recipes.Api.Tests.RepositoryTests
{
    using FluentAssertions;
    using CarbonKitchen.Recipes.Api.Data;
    using CarbonKitchen.Recipes.Api.Services;
    using CarbonKitchen.Recipes.Api.Tests.Fakes;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Linq;
    using Xunit;

    public class CreateRecipeRepositoryTests
    {
        [Fact]
        public void AddRecipe_NewRecordAddedWithProperValues()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipe = new FakeRecipe { }.Generate();

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                service.AddRecipe(fakeRecipe);

                context.SaveChanges();
            }

            //Assert
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.Count().Should().Be(1);

                var recipeById = context.Recipes.FirstOrDefault(r => r.RecipeId == fakeRecipe.RecipeId);

                recipeById.Should().BeEquivalentTo(fakeRecipe);
                recipeById.RecipeId.Should().Be(fakeRecipe.RecipeId);
                recipeById.Title.Should().Be(fakeRecipe.Title);
                recipeById.Directions.Should().Be(fakeRecipe.Directions);
                recipeById.RecipeDateField1.Should().Be(fakeRecipe.RecipeDateField1);
            }
        }
    }
}
