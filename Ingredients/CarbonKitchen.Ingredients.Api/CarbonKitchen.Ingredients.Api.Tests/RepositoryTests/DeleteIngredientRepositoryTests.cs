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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;
    using CarbonKitchen.Ingredients.Api.Data.Entities;
    using CarbonKitchen.Recipes.Api.Models.Pagination;
    using Microsoft.AspNetCore.Http.Features;

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

        [Fact]
        public void DeleteIngredientList_ReturnsProperCount()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<IngredientDbContext>()
                .UseInMemoryDatabase(databaseName: $"IngredientDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeIngredientOne = new FakeIngredient { }.Generate();
            var fakeIngredientTwo = new FakeIngredient { }.Generate();
            var fakeIngredientThree = new FakeIngredient { }.Generate();

            var deleteId = 1;
            fakeIngredientOne.RecipeId = deleteId;
            fakeIngredientTwo.RecipeId = deleteId;
            fakeIngredientThree.RecipeId = 2;

            //Act
            using (var context = new IngredientDbContext(dbOptions))
            {
                context.Ingredients.AddRange(fakeIngredientOne, fakeIngredientTwo, fakeIngredientThree);

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));
                var deleteList = new List<Ingredient>
                {
                    fakeIngredientOne,
                    fakeIngredientTwo
                };

                service.DeleteIngredients(deleteList);

                context.SaveChanges();

                //Assert
                var ingredientList = context.Ingredients.ToList();

                ingredientList.Should().ContainEquivalentOf(fakeIngredientThree);
                //Assert.DoesNotContain(ingredientList, i => i == fakeIngredientOne);
                //Assert.DoesNotContain(ingredientList, i => i == fakeIngredientTwo);
                ingredientList.Should().HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void DeleteIngredientListWithId_ReturnsProperCount()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<IngredientDbContext>()
                .UseInMemoryDatabase(databaseName: $"IngredientDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeIngredientOne = new FakeIngredient { }.Generate();
            var fakeIngredientTwo = new FakeIngredient { }.Generate();
            var fakeIngredientThree = new FakeIngredient { }.Generate();

            var deleteId = 1;
            fakeIngredientOne.RecipeId = deleteId;
            fakeIngredientTwo.RecipeId = deleteId;
            fakeIngredientThree.RecipeId = 2;

            //Act
            using (var context = new IngredientDbContext(dbOptions))
            {
                context.Ingredients.AddRange(fakeIngredientOne, fakeIngredientTwo, fakeIngredientThree);

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                service.DeleteIngredients(deleteId);

                context.SaveChanges();

                //Assert
                var ingredientList = context.Ingredients.ToList();

                ingredientList.Should().ContainEquivalentOf(fakeIngredientThree);
                //Assert.DoesNotContain(ingredientList, i => i == fakeIngredientOne);
                //Assert.DoesNotContain(ingredientList, i => i == fakeIngredientTwo);
                ingredientList.Should().HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }
    }
}
