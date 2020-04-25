namespace CarbonKitchen.Ingredients.Api.Tests.RepositoryTests
{
    using FluentAssertions;
    using CarbonKitchen.Ingredients.Api.Data;
    using CarbonKitchen.Ingredients.Api.Models;
    using CarbonKitchen.Ingredients.Api.Services;
    using CarbonKitchen.Ingredients.Api.Tests.Fakes;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Linq;
    using Xunit;

    public class GetIngredientRepositoryTests
    {
        [Fact]
        public void GetIngredient_ParametersMatchExpectedValues()
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
                context.Ingredients.AddRange(fakeIngredient);
                context.SaveChanges();

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                //Assert
                var ingredientById = context.Ingredients.FirstOrDefault(i => i.IngredientId == fakeIngredient.IngredientId);

                ingredientById.Should().BeEquivalentTo(fakeIngredient);
                ingredientById.IngredientId.Should().Be(fakeIngredient.IngredientId);
                ingredientById.Ingredient.Should().Be(fakeIngredient.Ingredient);
                ingredientById.IngredientTextField2.Should().Be(fakeIngredient.IngredientTextField2);
                ingredientById.IngredientDateField1.Should().Be(fakeIngredient.IngredientDateField1);
            }
        }

        [Fact]
        public void GetIngredients_CountMatchesAndContainsEvuivalentObjects()
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
                context.SaveChanges();

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                var ingredientRepo = service.GetIngredients(new IngredientParametersDto());

                //Assert
                ingredientRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                ingredientRepo.Should().ContainEquivalentOf(fakeIngredientOne);
                ingredientRepo.Should().ContainEquivalentOf(fakeIngredientTwo);
                ingredientRepo.Should().ContainEquivalentOf(fakeIngredientThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetIngredients_ReturnExpectedPageSize()
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
                context.SaveChanges();

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                var ingredientRepo = service.GetIngredients(new IngredientParametersDto { PageSize = 2 });

                //Assert
                ingredientRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                ingredientRepo.Should().ContainEquivalentOf(fakeIngredientOne);
                ingredientRepo.Should().ContainEquivalentOf(fakeIngredientTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetIngredients_ReturnExpectedPageNumberAndSize()
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
                context.SaveChanges();

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                var ingredientRepo = service.GetIngredients(new IngredientParametersDto { PageSize = 1, PageNumber = 2 });

                //Assert
                ingredientRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                ingredientRepo.Should().ContainEquivalentOf(fakeIngredientTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetIngredients_ListSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<IngredientDbContext>()
                .UseInMemoryDatabase(databaseName: $"IngredientDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeIngredientOne = new FakeIngredient { }.Generate();
            fakeIngredientOne.Ingredient = "Bravo";

            var fakeIngredientTwo = new FakeIngredient { }.Generate();
            fakeIngredientTwo.Ingredient = "Alpha";

            var fakeIngredientThree = new FakeIngredient { }.Generate();
            fakeIngredientThree.Ingredient = "Charlie";

            //Act
            using (var context = new IngredientDbContext(dbOptions))
            {
                context.Ingredients.AddRange(fakeIngredientOne, fakeIngredientTwo, fakeIngredientThree);
                context.SaveChanges();

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                var ingredientRepo = service.GetIngredients(new IngredientParametersDto { SortOrder = "Ingredient" });

                //Assert
                ingredientRepo.Should()
                    .ContainInOrder(fakeIngredientTwo, fakeIngredientOne, fakeIngredientThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetIngredients_ListSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<IngredientDbContext>()
                .UseInMemoryDatabase(databaseName: $"IngredientDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeIngredientOne = new FakeIngredient { }.Generate();
            fakeIngredientOne.Ingredient = "Bravo";

            var fakeIngredientTwo = new FakeIngredient { }.Generate();
            fakeIngredientTwo.Ingredient = "Alpha";

            var fakeIngredientThree = new FakeIngredient { }.Generate();
            fakeIngredientThree.Ingredient = "Charlie";

            //Act
            using (var context = new IngredientDbContext(dbOptions))
            {
                context.Ingredients.AddRange(fakeIngredientOne, fakeIngredientTwo, fakeIngredientThree);
                context.SaveChanges();

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                var ingredientRepo = service.GetIngredients(new IngredientParametersDto { SortOrder = "-Ingredient" });

                //Assert
                ingredientRepo.Should()
                    .ContainInOrder(fakeIngredientThree, fakeIngredientOne, fakeIngredientTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData("Ingredient == Alpha")]
        [InlineData("IngredientTextField2 == Bravo")]
        [InlineData("RecipeId == 5")]
        [InlineData("Ingredient == Charlie")]
        [InlineData("IngredientTextField2 == Delta")]
        [InlineData("RecipeId == 6")]
        [InlineData("Ingredient == Echo")]
        [InlineData("IngredientTextField2 == Foxtrot")]
        [InlineData("RecipeId == 7")]
        public void GetIngredients_FilterListWithExact(string filters)
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<IngredientDbContext>()
                .UseInMemoryDatabase(databaseName: $"IngredientDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeIngredientOne = new FakeIngredient { }.Generate();
            fakeIngredientOne.Ingredient = "Alpha";
            fakeIngredientOne.IngredientTextField2 = "Bravo";
            fakeIngredientOne.RecipeId = 5;

            var fakeIngredientTwo = new FakeIngredient { }.Generate();
            fakeIngredientTwo.Ingredient = "Charlie";
            fakeIngredientTwo.IngredientTextField2 = "Delta";
            fakeIngredientTwo.RecipeId = 6;

            var fakeIngredientThree = new FakeIngredient { }.Generate();
            fakeIngredientThree.Ingredient = "Echo";
            fakeIngredientThree.IngredientTextField2 = "Foxtrot";
            fakeIngredientThree.RecipeId = 7;

            //Act
            using (var context = new IngredientDbContext(dbOptions))
            {
                context.Ingredients.AddRange(fakeIngredientOne, fakeIngredientTwo, fakeIngredientThree);
                context.SaveChanges();

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                var ingredientRepo = service.GetIngredients(new IngredientParametersDto { Filters = filters });

                //Assert
                ingredientRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData("Ingredient@=Hart", 1)]
        [InlineData("IngredientTextField2@=Fav", 1)]
        [InlineData("Ingredient@=*hart", 2)]
        [InlineData("IngredientTextField2@=*fav", 2)]
        public void GetIngredients_FilterListWithContains(string filters, int expectedCount)
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<IngredientDbContext>()
                .UseInMemoryDatabase(databaseName: $"IngredientDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeIngredientOne = new FakeIngredient { }.Generate();
            fakeIngredientOne.Ingredient = "Alpha";
            fakeIngredientOne.IngredientTextField2 = "Bravo";

            var fakeIngredientTwo = new FakeIngredient { }.Generate();
            fakeIngredientTwo.Ingredient = "Hartsfield";
            fakeIngredientTwo.IngredientTextField2 = "Favaro";

            var fakeIngredientThree = new FakeIngredient { }.Generate();
            fakeIngredientThree.Ingredient = "Bravehart";
            fakeIngredientThree.IngredientTextField2 = "Jonfav";

            //Act
            using (var context = new IngredientDbContext(dbOptions))
            {
                context.Ingredients.AddRange(fakeIngredientOne, fakeIngredientTwo, fakeIngredientThree);
                context.SaveChanges();

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                var ingredientRepo = service.GetIngredients(new IngredientParametersDto { Filters = filters });

                //Assert
                ingredientRepo.Should()
                    .HaveCount(expectedCount);

                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData("hart", 1)]
        [InlineData("fav", 1)]
        [InlineData("Fav", 0)]
        public void GetIngredients_SearchQueryReturnsExpectedRecordCount(string queryString, int expectedCount)
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<IngredientDbContext>()
                .UseInMemoryDatabase(databaseName: $"IngredientDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeIngredientOne = new FakeIngredient { }.Generate();
            fakeIngredientOne.Ingredient = "Alpha";
            fakeIngredientOne.IngredientTextField2 = "Bravo";

            var fakeIngredientTwo = new FakeIngredient { }.Generate();
            fakeIngredientTwo.Ingredient = "Hartsfield";
            fakeIngredientTwo.IngredientTextField2 = "White";

            var fakeIngredientThree = new FakeIngredient { }.Generate();
            fakeIngredientThree.Ingredient = "Bravehart";
            fakeIngredientThree.IngredientTextField2 = "Jonfav";

            //Act
            using (var context = new IngredientDbContext(dbOptions))
            {
                context.Ingredients.AddRange(fakeIngredientOne, fakeIngredientTwo, fakeIngredientThree);
                context.SaveChanges();

                var service = new IngredientRepository(context, new SieveProcessor(sieveOptions));

                var ingredientRepo = service.GetIngredients(new IngredientParametersDto { QueryString = queryString });

                //Assert
                ingredientRepo.Should()
                    .HaveCount(expectedCount);

                context.Database.EnsureDeleted();
            }
        }
    }
}
