namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Commands
{
    using MediatR;

    public class DeleteShoppingListItemCommand : IRequest<bool>
    {
        public int ShoppingListItemId { get; }

        public DeleteShoppingListItemCommand(int shoppingListItemsId)
        {
            ShoppingListItemId = shoppingListItemsId;
        }
    }
}
