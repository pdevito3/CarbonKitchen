namespace CarbonKitchen.ShoppingListItems.Api.Tests.RepositoryTests
{
    using FluentAssertions;
    using CarbonKitchen.ShoppingListItems.Api.Data;
    using CarbonKitchen.ShoppingListItems.Api.Services;
    using CarbonKitchen.ShoppingListItems.Api.Tests.Fakes;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class DeleteShoppingListItemRepositoryTests
    {
        [Fact]
        public void DeleteShoppingListItem_ReturnsProperCount()
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

                var service = new ShoppingListItemRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteShoppingListItem(fakeShoppingListItemTwo);

                context.SaveChanges();

                //Assert
                var shoppingListItemsList = context.ShoppingListItems.ToList();

                shoppingListItemsList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                shoppingListItemsList.Should().ContainEquivalentOf(fakeShoppingListItemOne);
                shoppingListItemsList.Should().ContainEquivalentOf(fakeShoppingListItemThree);
                Assert.DoesNotContain(shoppingListItemsList, sli => sli == fakeShoppingListItemTwo);

                context.Database.EnsureDeleted();
            }
        }
    }
}
