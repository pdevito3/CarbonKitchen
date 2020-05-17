namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Commands
{
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using MediatR;

    public class UpdateEntireShoppingListItemCommand : IRequest<string>
    {
        public int ShoppingListItemId { get; set; }
        public ShoppingListItemForUpdateDto ShoppingListItemForUpdateDto { get; set; }

        public UpdateEntireShoppingListItemCommand(int shoppingListItemsId,ShoppingListItemForUpdateDto shoppingListItemsForUpdateDto)
        {
            ShoppingListItemId = shoppingListItemsId;
            ShoppingListItemForUpdateDto = shoppingListItemsForUpdateDto;
        }
    }
}
