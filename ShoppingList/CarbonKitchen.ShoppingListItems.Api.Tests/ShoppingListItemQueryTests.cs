namespace CarbonKitchen.ShoppingListItems.Api.Tests
{
    using FluentAssertions;
    using CarbonKitchen.ShoppingListItems.Api.Controllers;
    using CarbonKitchen.ShoppingListItems.Api.Data;
    using CarbonKitchen.ShoppingListItems.Api.Mediator.Queries;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Tests.Fakes;
    using MediatR;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ShoppingListItemQueryTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public ShoppingListItemQueryTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly CustomWebApplicationFactory<Startup> _factory;

        [Fact]
        public async Task GetShoppingListItemQuery_ReturnsResourceWithProperFields()
        {
            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ShoppingListItemDbContext>();
            context.Database.EnsureCreated();
            context.ShoppingListItems.RemoveRange(context.ShoppingListItems); // change this to use respawn?

            context.ShoppingListItems.AddRange(fakeShoppingListItemOne);
            context.SaveChanges();

            var query = new GetShoppingListItemQuery(fakeShoppingListItemOne.ShoppingListItemId);
            var result = await mediator.Send(query);

            result.Should().BeEquivalentTo(fakeShoppingListItemOne);
        }

        [Fact]
        public async Task GetAllShoppingListItemsQuery_ReturnsResourcesWithProperFields()
        {
            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
            var fakeShoppingListItemTwo = new FakeShoppingListItem { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ShoppingListItemDbContext>();
            context.Database.EnsureCreated();
            context.ShoppingListItems.RemoveRange(context.ShoppingListItems); // change this to use respawn?

            context.ShoppingListItems.AddRange(fakeShoppingListItemOne, fakeShoppingListItemTwo);
            context.SaveChanges();

            var shoppingListItemsParametersDto = new ShoppingListItemParametersDto { };
            var query = new GetAllShoppingListItemsQuery(shoppingListItemsParametersDto, new ShoppingListItemsController(mediator));
            var result = await mediator.Send(query);

            result.ShoppingListItemDtoList.Should().HaveCount(2);
            result.ShoppingListItemDtoList.Should().ContainEquivalentOf(fakeShoppingListItemOne);
            result.ShoppingListItemDtoList.Should().ContainEquivalentOf(fakeShoppingListItemTwo);
        }

        [Fact]
        public async Task GetAllShoppingListItemsQuery_ReturnsHeaderWithNextPageAndPreviousPageBoolWhenExpectedWithPageInfo()
        {
            var fakeShoppingListItemOne = new FakeShoppingListItem { }.Generate();
            var fakeShoppingListItemTwo = new FakeShoppingListItem { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ShoppingListItemDbContext>();
            context.Database.EnsureCreated();
            context.ShoppingListItems.RemoveRange(context.ShoppingListItems); // change this to use respawn?

            context.ShoppingListItems.AddRange(fakeShoppingListItemOne, fakeShoppingListItemTwo);
            context.SaveChanges();

            var shoppingListItemsParametersDto = new ShoppingListItemParametersDto { PageSize = 1 };
            var controller = new ShoppingListItemsController(mediator);
            var mockUrl = new Mock<IUrlHelper>();
            controller.Url = mockUrl.Object;

            var query = new GetAllShoppingListItemsQuery(shoppingListItemsParametersDto, controller);
            var result = await mediator.Send(query);

            result.PaginationMetadata.HasNext.Should().Be(true);
            result.PaginationMetadata.HasPrevious.Should().Be(false);
        }

        //TODO: Add tests for PaginationMetadata.NextPageLink, PaginationMetadata.PreviousLink
        /* possible mocks for controllers
         
            //var mockUrl = new Mock<IUrlHelper>(MockBehavior.Strict);
            //Expression<Func<IUrlHelper, string>> urlSetup
            //    = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
            //mockUrl.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();
            //mockUrl.SetupAllProperties();

         */
    }
}
