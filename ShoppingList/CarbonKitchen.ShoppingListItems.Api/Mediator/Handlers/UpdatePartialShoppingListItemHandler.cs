namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Handlers
{
    using AutoMapper;
    using CarbonKitchen.ShoppingListItems.Api.Data.Entities;
    using CarbonKitchen.ShoppingListItems.Api.Mediator.Commands;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Services;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdatePartialShoppingListItemHandler : IRequestHandler<UpdatePartialShoppingListItemCommand, IActionResult>
    {
        private readonly IShoppingListItemRepository _shoppingListItemsRepository;
        private readonly IMapper _mapper;

        public UpdatePartialShoppingListItemHandler(IShoppingListItemRepository shoppingListItemsRepository
            , IMapper mapper)
        {
            _shoppingListItemsRepository = shoppingListItemsRepository ??
                throw new ArgumentNullException(nameof(shoppingListItemsRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Handle(UpdatePartialShoppingListItemCommand updatePartialShoppingListItemCommand, CancellationToken cancellationToken)
        {
            if (updatePartialShoppingListItemCommand.PatchDoc == null)
            {
                return updatePartialShoppingListItemCommand.Controller.BadRequest();
            }

            var existingShoppingListItem = _shoppingListItemsRepository.GetShoppingListItem(updatePartialShoppingListItemCommand.ShoppingListItemId);

            if (existingShoppingListItem == null)
            {
                return updatePartialShoppingListItemCommand.Controller.NotFound();
            }

            var shoppingListItemsToPatch = _mapper.Map<ShoppingListItemForUpdateDto>(existingShoppingListItem); // map the shoppingListItems we got from the database to an updatable shoppingListItems model
            updatePartialShoppingListItemCommand.PatchDoc.ApplyTo(shoppingListItemsToPatch, updatePartialShoppingListItemCommand.Controller.ModelState); // apply patchdoc updates to the updatable shoppingListItems

            if (!updatePartialShoppingListItemCommand.Controller.TryValidateModel(shoppingListItemsToPatch))
            {
                return updatePartialShoppingListItemCommand.Controller.ValidationProblem(updatePartialShoppingListItemCommand.Controller.ModelState);
            }

            _mapper.Map(shoppingListItemsToPatch, existingShoppingListItem); // apply updates from the updatable shoppingListItems to the db entity so we can apply the updates to the database
            _shoppingListItemsRepository.UpdateShoppingListItem(existingShoppingListItem); // apply business updates to data if needed

            _shoppingListItemsRepository.Save(); // save changes in the database

            return updatePartialShoppingListItemCommand.Controller.NoContent();
        }
    }
}
