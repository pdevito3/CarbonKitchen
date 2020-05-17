namespace CarbonKitchen.ShoppingListItems.Api.Tests
{
    using AutoBogus;
    using AutoMapper;
    using FluentAssertions;
    using CarbonKitchen.ShoppingListItems.Api.Data;
    using CarbonKitchen.ShoppingListItems.Api.Mediator.Commands;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Tests.Fakes;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class ShoppingListItemCommandTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public ShoppingListItemCommandTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly CustomWebApplicationFactory<Startup> _factory;

        [Fact]
        public async Task CreateShoppingListItem_NewShoppingListItemAddedAndReturned()
        {
            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();
            var context = scope.ServiceProvider.GetRequiredService<ShoppingListItemDbContext>();
            context.Database.EnsureCreated();
            context.ShoppingListItems.RemoveRange(context.ShoppingListItems); // change this to use respawn?

            var shoppingListItemsForCreationDto = mapper.Map<ShoppingListItemForCreationDto>(fakeShoppingListItemOne);
            var query = new CreateShoppingListItemCommand(shoppingListItemsForCreationDto);
            var result = await mediator.Send(query);
            fakeShoppingListItemOne.ShoppingListItemId = result.ShoppingListItemId;

            var shoppingListItemsDtoFromRepo = context.ShoppingListItems.ToList();

            shoppingListItemsDtoFromRepo.Should().HaveCount(1);
            shoppingListItemsDtoFromRepo.Should().ContainEquivalentOf(result);
            fakeShoppingListItemOne.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task DeleteShoppingListItem_RemovesShoppingListItem()
        {
            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ShoppingListItemDbContext>();
            context.Database.EnsureCreated();
            context.ShoppingListItems.RemoveRange(context.ShoppingListItems); // change this to use respawn?
            context.ShoppingListItems.Add(fakeShoppingListItemOne);
            context.SaveChanges();

            var shoppingListItemsDtoFromRepo = context.ShoppingListItems.FirstOrDefault();

            var query = new DeleteShoppingListItemCommand(shoppingListItemsDtoFromRepo.ShoppingListItemId);
            var result = await mediator.Send(query);

            var shoppingListItemsDtosFromRepo = context.ShoppingListItems.ToList();

            shoppingListItemsDtosFromRepo.Should().HaveCount(0);
            shoppingListItemsDtosFromRepo.Should().NotContain(shoppingListItemsDtoFromRepo);
        }

        [Fact]
        public async Task DeleteShoppingListItemWithNonExistantId_DoesNotRemoveRemovesShoppingListItem()
        {
            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ShoppingListItemDbContext>();
            context.Database.EnsureCreated();
            context.ShoppingListItems.RemoveRange(context.ShoppingListItems); // change this to use respawn?
            context.ShoppingListItems.Add(fakeShoppingListItemOne);
            context.SaveChanges();

            var shoppingListItemsDtoFromRepo = context.ShoppingListItems.FirstOrDefault();

            var query = new DeleteShoppingListItemCommand(0);
            _ = await mediator.Send(query);

            var shoppingListItemsDtosFromRepo = context.ShoppingListItems.ToList();

            shoppingListItemsDtosFromRepo.Should().HaveCount(1);
            shoppingListItemsDtosFromRepo.Should().ContainEquivalentOf(shoppingListItemsDtoFromRepo);
        }

        [Fact]
        public async Task UpdateEntireShoppingListItem_NewShoppingListItemAddedAndReturned()
        {
            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
            var shoppingListItemsForUpdateDto = new AutoFaker<ShoppingListItemForUpdateDto> { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ShoppingListItemDbContext>();
            context.Database.EnsureCreated();
            context.ShoppingListItems.RemoveRange(context.ShoppingListItems); // change this to use respawn?
            context.ShoppingListItems.Add(fakeShoppingListItemOne);
            context.SaveChanges();

            var shoppingListItemsDtoFromRepo = context.ShoppingListItems.FirstOrDefault();
            var query = new UpdateEntireShoppingListItemCommand(shoppingListItemsDtoFromRepo.ShoppingListItemId, shoppingListItemsForUpdateDto);
            _ = await mediator.Send(query);

            shoppingListItemsDtoFromRepo = context.ShoppingListItems.FirstOrDefault();

            shoppingListItemsDtoFromRepo.Name.Should().Be(shoppingListItemsForUpdateDto.Name);
            shoppingListItemsDtoFromRepo.ShoppingListId.Should().Be(shoppingListItemsForUpdateDto.ShoppingListId);
            shoppingListItemsDtoFromRepo.Amount.Should().Be(shoppingListItemsForUpdateDto.Amount);
        }

        //TODO: figure out better controller mocking for patch
        //[Fact]
        //public async Task UpdatePartialShoppingListItem_NewShoppingListItemAddedAndReturned()
        //{
        //    var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
        //    var shoppingListItemsPatchDoc = new JsonPatchDocument<ShoppingListItemForUpdateDto> { };
        //    shoppingListItemsPatchDoc.Replace(p => p.Name, "New Val");

        //    var scope = _factory.Services.CreateScope();

        //    var mediator = scope.ServiceProvider.GetService<IMediator>();
        //    var context = scope.ServiceProvider.GetRequiredService<ShoppingListItemDbContext>();
        //    context.Database.EnsureCreated();
        //    context.ShoppingListItems.RemoveRange(context.ShoppingListItems); // change this to use respawn?
        //    context.ShoppingListItems.Add(fakeShoppingListItemOne);
        //    context.SaveChanges();

        //    var controller = new ShoppingListItemsController(mediator);
        //    var shoppingListItemsDtoFromRepo = context.ShoppingListItems.FirstOrDefault();
        //    var query = new UpdatePartialShoppingListItemCommand(shoppingListItemsDtoFromRepo.ShoppingListItemId, shoppingListItemsPatchDoc, controller);
        //    _ = await mediator.Send(query);

        //    shoppingListItemsDtoFromRepo = context.ShoppingListItems.FirstOrDefault();

        //    shoppingListItemsDtoFromRepo.Name.Should().Be("New Val");
        //}
    }
}
