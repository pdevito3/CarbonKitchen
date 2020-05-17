namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Commands
{
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using MediatR;

    public class CreateShoppingListItemCommand : IRequest<ShoppingListItemDto>
    {
        public ShoppingListItemForCreationDto ShoppingListItemForCreationDto { get; }

        public CreateShoppingListItemCommand(ShoppingListItemForCreationDto shoppingListItemsForCreationDto)
        {
            ShoppingListItemForCreationDto = shoppingListItemsForCreationDto;
        }
    }
}
