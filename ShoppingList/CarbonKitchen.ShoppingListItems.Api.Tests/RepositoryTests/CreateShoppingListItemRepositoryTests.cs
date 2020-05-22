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
    using System.Linq;
    using Xunit;

    public class CreateShoppingListItemRepositoryTests
    {
        [Fact]
        public void AddShoppingListItem_NewRecordAddedWithProperValues()
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
                var service = new ShoppingListItemRepository(context, new SieveProcessor(sieveOptions));

                service.AddShoppingListItem(fakeShoppingListItem);

                context.SaveChanges();
            }

            //Assert
            using (var context = new ShoppingListItemDbContext(dbOptions))
            {
                context.ShoppingListItems.Count().Should().Be(1);

                var shoppingListItemById = context.ShoppingListItems.FirstOrDefault(sli => sli.ShoppingListItemId == fakeShoppingListItem.ShoppingListItemId);

                shoppingListItemById.Should().BeEquivalentTo(fakeShoppingListItem);
                shoppingListItemById.ShoppingListItemId.Should().Be(fakeShoppingListItem.ShoppingListItemId);
                shoppingListItemById.Name.Should().Be(fakeShoppingListItem.Name);
                shoppingListItemById.Category.Should().Be(fakeShoppingListItem.Category);
                shoppingListItemById.ShoppingListItemDateField1.Should().Be(fakeShoppingListItem.ShoppingListItemDateField1);
            }
        }
    }
}
