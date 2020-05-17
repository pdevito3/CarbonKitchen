namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Handlers
{
    using AutoMapper;
    using CarbonKitchen.ShoppingListItems.Api.Data.Entities;
    using CarbonKitchen.ShoppingListItems.Api.Mediator.Commands;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Services;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateEntireShoppingListItemHandler : IRequestHandler<UpdateEntireShoppingListItemCommand, string>
    {
        private readonly IShoppingListItemRepository _shoppingListItemsRepository;
        private readonly IMapper _mapper;

        public UpdateEntireShoppingListItemHandler(IShoppingListItemRepository shoppingListItemsRepository
            , IMapper mapper)
        {
            _shoppingListItemsRepository = shoppingListItemsRepository ??
                throw new ArgumentNullException(nameof(shoppingListItemsRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<string> Handle(UpdateEntireShoppingListItemCommand updateEntireShoppingListItemCommand, CancellationToken cancellationToken)
        {
            var shoppingListItemsFromRepo = _shoppingListItemsRepository.GetShoppingListItem(updateEntireShoppingListItemCommand.ShoppingListItemId);

            if (shoppingListItemsFromRepo == null)
            {
                return "NotFound";
            }

            _mapper.Map(updateEntireShoppingListItemCommand.ShoppingListItemForUpdateDto, shoppingListItemsFromRepo);
            _shoppingListItemsRepository.UpdateShoppingListItem(shoppingListItemsFromRepo);

            _shoppingListItemsRepository.Save();

            return "NoContent";
        }
    }
}
