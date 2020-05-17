namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using CarbonKitchen.ShoppingListItems.Api.Mediator.Commands;
    using CarbonKitchen.ShoppingListItems.Api.Mediator.Queries;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Services;
    using MediatR;

    public class DeleteShoppingListItemHandler : IRequestHandler<DeleteShoppingListItemCommand, bool>
    {
        private readonly IShoppingListItemRepository _shoppingListItemsRepository;
        private readonly IMapper _mapper;

        public DeleteShoppingListItemHandler(IShoppingListItemRepository shoppingListItemsRepository
            , IMapper mapper)
        {
            _shoppingListItemsRepository = shoppingListItemsRepository ??
                throw new ArgumentNullException(nameof(shoppingListItemsRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> Handle(DeleteShoppingListItemCommand command, CancellationToken cancellationToken)
        {
            var shoppingListItemsFromRepo = _shoppingListItemsRepository.GetShoppingListItem(command.ShoppingListItemId);

            if (shoppingListItemsFromRepo == null)
            {
                return false;
            }

            _shoppingListItemsRepository.DeleteShoppingListItem(shoppingListItemsFromRepo);
            _shoppingListItemsRepository.Save();

            return true;
        }
    }
}
