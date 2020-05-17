﻿namespace CarbonKitchen.ShoppingListItems.Api.Tests.RepositoryTests
{
    using FluentAssertions;
    using CarbonKitchen.ShoppingListItems.Api.Data;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Services;
    using CarbonKitchen.ShoppingListItems.Api.Tests.Fakes;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Linq;
    using Xunit;

    public class GetShoppingListItemRepositoryTests
    {
        [Fact]
        public void GetShoppingListItem_ParametersMatchExpectedValues()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ShoppingListItemDbContext>()
                .UseInMemoryDatabase(databaseName: $"ShoppingListItemDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeShoppingListItem = new FakeShoppingListItem { }.Generate();

            //Act
            using (var context = new ShoppingListItemDbContext(dbOptions))
            {
                context.ShoppingListItems.AddRange(fakeShoppingListItem);
                context.SaveChanges();

                var service = new ShoppingListItemRepository(context, new SieveProcessor(sieveOptions));

                //Assert
                var shoppingListItemsById = context.ShoppingListItems.FirstOrDefault(sli => sli.ShoppingListItemId == fakeShoppingListItem.ShoppingListItemId);

                shoppingListItemsById.Should().BeEquivalentTo(fakeShoppingListItem);
                shoppingListItemsById.ShoppingListItemId.Should().Be(fakeShoppingListItem.ShoppingListItemId);
                shoppingListItemsById.Name.Should().Be(fakeShoppingListItem.Name);
            }
        }

        [Fact]
        public void GetShoppingListItems_CountMatchesAndContainsEvuivalentObjects()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ShoppingListItemDbContext>()
                .UseInMemoryDatabase(databaseName: $"ShoppingListItemDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
            var fakeShoppingListItemTwo = new FakeShoppingListItem { }.Generate();
            var fakeShoppingListItemThree = new FakeShoppingListItem { }.Generate();

            //Act
            using (var context = new ShoppingListItemDbContext(dbOptions))
            {
                context.ShoppingListItems.AddRange(fakeShoppingListItemOne, fakeShoppingListItemTwo, fakeShoppingListItemThree);
                context.SaveChanges();

                var service = new ShoppingListItemRepository(context, new SieveProcessor(sieveOptions));

                var shoppingListItemsRepo = service.GetShoppingListItems(new ShoppingListItemParametersDto());

                //Assert
                shoppingListItemsRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                shoppingListItemsRepo.Should().ContainEquivalentOf(fakeShoppingListItemOne);
                shoppingListItemsRepo.Should().ContainEquivalentOf(fakeShoppingListItemTwo);
                shoppingListItemsRepo.Should().ContainEquivalentOf(fakeShoppingListItemThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetShoppingListItems_ReturnExpectedPageSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ShoppingListItemDbContext>()
                .UseInMemoryDatabase(databaseName: $"ShoppingListItemDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
            var fakeShoppingListItemTwo = new FakeShoppingListItem { }.Generate();
            var fakeShoppingListItemThree = new FakeShoppingListItem { }.Generate();

            //Act
            using (var context = new ShoppingListItemDbContext(dbOptions))
            {
                context.ShoppingListItems.AddRange(fakeShoppingListItemOne, fakeShoppingListItemTwo, fakeShoppingListItemThree);
                context.SaveChanges();

                var service = new ShoppingListItemRepository(context, new SieveProcessor(sieveOptions));

                var shoppingListItemsRepo = service.GetShoppingListItems(new ShoppingListItemParametersDto { PageSize = 2 });

                //Assert
                shoppingListItemsRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                shoppingListItemsRepo.Should().ContainEquivalentOf(fakeShoppingListItemOne);
                shoppingListItemsRepo.Should().ContainEquivalentOf(fakeShoppingListItemTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetShoppingListItems_ReturnExpectedPageNumberAndSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ShoppingListItemDbContext>()
                .UseInMemoryDatabase(databaseName: $"ShoppingListItemDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
            var fakeShoppingListItemTwo = new FakeShoppingListItem { }.Generate();
            var fakeShoppingListItemThree = new FakeShoppingListItem { }.Generate();

            //Act
            using (var context = new ShoppingListItemDbContext(dbOptions))
            {
                context.ShoppingListItems.AddRange(fakeShoppingListItemOne, fakeShoppingListItemTwo, fakeShoppingListItemThree);
                context.SaveChanges();

                var service = new ShoppingListItemRepository(context, new SieveProcessor(sieveOptions));

                var shoppingListItemsRepo = service.GetShoppingListItems(new ShoppingListItemParametersDto { PageSize = 1, PageNumber = 2 });

                //Assert
                shoppingListItemsRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                shoppingListItemsRepo.Should().ContainEquivalentOf(fakeShoppingListItemTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetShoppingListItems_ListSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ShoppingListItemDbContext>()
                .UseInMemoryDatabase(databaseName: $"ShoppingListItemDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemOne.Name = "Bravo";

            var fakeShoppingListItemTwo = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemTwo.Name = "Alpha";

            var fakeShoppingListItemThree = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemThree.Name = "Charlie";

            //Act
            using (var context = new ShoppingListItemDbContext(dbOptions))
            {
                context.ShoppingListItems.AddRange(fakeShoppingListItemOne, fakeShoppingListItemTwo, fakeShoppingListItemThree);
                context.SaveChanges();

                var service = new ShoppingListItemRepository(context, new SieveProcessor(sieveOptions));

                var shoppingListItemsRepo = service.GetShoppingListItems(new ShoppingListItemParametersDto { SortOrder = "Name" });

                //Assert
                shoppingListItemsRepo.Should()
                    .ContainInOrder(fakeShoppingListItemTwo, fakeShoppingListItemOne, fakeShoppingListItemThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetShoppingListItems_ListSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ShoppingListItemDbContext>()
                .UseInMemoryDatabase(databaseName: $"ShoppingListItemDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemOne.Name = "Bravo";

            var fakeShoppingListItemTwo = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemTwo.Name = "Alpha";

            var fakeShoppingListItemThree = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemThree.Name = "Charlie";

            //Act
            using (var context = new ShoppingListItemDbContext(dbOptions))
            {
                context.ShoppingListItems.AddRange(fakeShoppingListItemOne, fakeShoppingListItemTwo, fakeShoppingListItemThree);
                context.SaveChanges();

                var service = new ShoppingListItemRepository(context, new SieveProcessor(sieveOptions));

                var shoppingListItemsRepo = service.GetShoppingListItems(new ShoppingListItemParametersDto { SortOrder = "-Name" });

                //Assert
                shoppingListItemsRepo.Should()
                    .ContainInOrder(fakeShoppingListItemThree, fakeShoppingListItemOne, fakeShoppingListItemTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData("Name == Alpha")]
        [InlineData("Amount == 5")]
        [InlineData("Name == Charlie")]
        [InlineData("Amount == 6")]
        [InlineData("Name == Echo")]
        [InlineData("Amount == 7")]
        public void GetShoppingListItems_FilterListWithExact(string filters)
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ShoppingListItemDbContext>()
                .UseInMemoryDatabase(databaseName: $"ShoppingListItemDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemOne.Name = "Alpha";
            fakeShoppingListItemOne.Amount = 5;

            var fakeShoppingListItemTwo = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemTwo.Name = "Charlie";
            fakeShoppingListItemTwo.Amount = 6;

            var fakeShoppingListItemThree = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemThree.Name = "Echo";
            fakeShoppingListItemThree.Amount = 7;

            //Act
            using (var context = new ShoppingListItemDbContext(dbOptions))
            {
                context.ShoppingListItems.AddRange(fakeShoppingListItemOne, fakeShoppingListItemTwo, fakeShoppingListItemThree);
                context.SaveChanges();

                var service = new ShoppingListItemRepository(context, new SieveProcessor(sieveOptions));

                var shoppingListItemsRepo = service.GetShoppingListItems(new ShoppingListItemParametersDto { Filters = filters });

                //Assert
                shoppingListItemsRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData("Name@=Hart", 1)]
        [InlineData("Name@=*hart", 2)]
        public void GetShoppingListItems_FilterListWithContains(string filters, int expectedCount)
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ShoppingListItemDbContext>()
                .UseInMemoryDatabase(databaseName: $"ShoppingListItemDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemOne.Name = "Alpha";

            var fakeShoppingListItemTwo = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemTwo.Name = "Hartsfield";

            var fakeShoppingListItemThree = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemThree.Name = "Bravehart";

            //Act
            using (var context = new ShoppingListItemDbContext(dbOptions))
            {
                context.ShoppingListItems.AddRange(fakeShoppingListItemOne, fakeShoppingListItemTwo, fakeShoppingListItemThree);
                context.SaveChanges();

                var service = new ShoppingListItemRepository(context, new SieveProcessor(sieveOptions));

                var shoppingListItemsRepo = service.GetShoppingListItems(new ShoppingListItemParametersDto { Filters = filters });

                //Assert
                shoppingListItemsRepo.Should()
                    .HaveCount(expectedCount);

                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData("hart", 1)]
        [InlineData("fav", 1)]
        [InlineData("Fav", 0)]
        public void GetShoppingListItems_SearchQueryReturnsExpectedRecordCount(string queryString, int expectedCount)
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ShoppingListItemDbContext>()
                .UseInMemoryDatabase(databaseName: $"ShoppingListItemDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemOne.Name = "Alpha";

            var fakeShoppingListItemTwo = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemTwo.Name = "Hartsfield";

            var fakeShoppingListItemThree = new FakeShoppingListItem { }.Generate();
            fakeShoppingListItemThree.Name = "Bravehart";

            //Act
            using (var context = new ShoppingListItemDbContext(dbOptions))
            {
                context.ShoppingListItems.AddRange(fakeShoppingListItemOne, fakeShoppingListItemTwo, fakeShoppingListItemThree);
                context.SaveChanges();

                var service = new ShoppingListItemRepository(context, new SieveProcessor(sieveOptions));

                var shoppingListItemsRepo = service.GetShoppingListItems(new ShoppingListItemParametersDto { QueryString = queryString });

                //Assert
                shoppingListItemsRepo.Should()
                    .HaveCount(expectedCount);

                context.Database.EnsureDeleted();
            }
        }
    }
}
