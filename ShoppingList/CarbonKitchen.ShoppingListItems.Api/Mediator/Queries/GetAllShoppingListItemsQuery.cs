namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Queries
{
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Models.Pagination;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class GetAllShoppingListItemsQuery : IRequest<GetAllShoppingListItemQueryResponse>
    {
        public ShoppingListItemParametersDto ShoppingListItemParametersDto { get; }
        public Controller Controller { get; }

        public GetAllShoppingListItemsQuery(ShoppingListItemParametersDto shoppingListItemsParametersDto, Controller controller)
        {
            ShoppingListItemParametersDto = shoppingListItemsParametersDto;
            Controller = controller;
        }
    }
}
