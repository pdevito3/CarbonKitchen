namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using CarbonKitchen.ShoppingListItems.Api.Mediator.Queries;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Services;
    using MediatR;

    public class GetShoppingListItemHandler : IRequestHandler<GetShoppingListItemQuery, ShoppingListItemDto>
    {
        private readonly IShoppingListItemRepository _shoppingListItemsRepository;
        private readonly IMapper _mapper;

        public GetShoppingListItemHandler(IShoppingListItemRepository shoppingListItemsRepository
            , IMapper mapper)
        {
            _shoppingListItemsRepository = shoppingListItemsRepository ??
                throw new ArgumentNullException(nameof(shoppingListItemsRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ShoppingListItemDto> Handle(GetShoppingListItemQuery request, CancellationToken cancellationToken)
        {
            var shoppingListItemsFromRepo = _shoppingListItemsRepository.GetShoppingListItem(request.ShoppingListItemId);

            if (shoppingListItemsFromRepo == null)
            {
                return null;
            }

            var shoppingListItemsDto = _mapper.Map<ShoppingListItemDto>(shoppingListItemsFromRepo);

            return shoppingListItemsDto;
        }
    }
}
