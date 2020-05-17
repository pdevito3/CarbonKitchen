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

    public class CreateShoppingListItemHandler : IRequestHandler<CreateShoppingListItemCommand, ShoppingListItemDto>
    {
        private readonly IShoppingListItemRepository _shoppingListItemsRepository;
        private readonly IMapper _mapper;

        public CreateShoppingListItemHandler(IShoppingListItemRepository shoppingListItemsRepository
            , IMapper mapper)
        {
            _shoppingListItemsRepository = shoppingListItemsRepository ??
                throw new ArgumentNullException(nameof(shoppingListItemsRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ShoppingListItemDto> Handle(CreateShoppingListItemCommand createShoppingListItemCommand, CancellationToken cancellationToken)
        {
            var shoppingListItems = _mapper.Map<ShoppingListItem>(createShoppingListItemCommand.ShoppingListItemForCreationDto);
            _shoppingListItemsRepository.AddShoppingListItem(shoppingListItems);
            _shoppingListItemsRepository.Save();

            return _mapper.Map<ShoppingListItemDto>(shoppingListItems);
        }
    }
}
