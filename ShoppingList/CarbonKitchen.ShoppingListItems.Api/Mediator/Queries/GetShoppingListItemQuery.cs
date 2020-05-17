namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Queries
{
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using MediatR;

    public class GetShoppingListItemQuery : IRequest<ShoppingListItemDto>
    {
        public int ShoppingListItemId { get; }

        public GetShoppingListItemQuery(int shoppingListItemsId)
        {
            ShoppingListItemId = shoppingListItemsId;
        }
    }
}
