namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Commands
{
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using MediatR;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;

    public class UpdatePartialShoppingListItemCommand : IRequest<IActionResult>
    {
        public int ShoppingListItemId { get; set; }
        public JsonPatchDocument<ShoppingListItemForUpdateDto> PatchDoc { get; set; }
        public Controller Controller { get; set; }

        public UpdatePartialShoppingListItemCommand(int shoppingListItemsId, JsonPatchDocument<ShoppingListItemForUpdateDto> patchDoc,
            Controller controller)
        {
            ShoppingListItemId = shoppingListItemsId;
            PatchDoc = patchDoc;
            Controller = controller;
        }
    }
}
