namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using CarbonKitchen.ShoppingListItems.Api.Mediator.Queries;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Models.Pagination;
    using CarbonKitchen.ShoppingListItems.Api.Services;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class GetAllShoppingListItemsHandler : IRequestHandler<GetAllShoppingListItemsQuery, GetAllShoppingListItemQueryResponse> // left is what want want to get in, right is what we want to send out
    {
        private readonly IShoppingListItemRepository _shoppingListItemsRepository;
        private readonly IMapper _mapper;

        public GetAllShoppingListItemsHandler(IShoppingListItemRepository shoppingListItemsRepository
            , IMapper mapper)
        {
            _shoppingListItemsRepository = shoppingListItemsRepository ??
                throw new ArgumentNullException(nameof(shoppingListItemsRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetAllShoppingListItemQueryResponse> Handle(GetAllShoppingListItemsQuery request, CancellationToken cancellationToken)
        {
            var shoppingListItemssFromRepo = _shoppingListItemsRepository.GetShoppingListItems(request.ShoppingListItemParametersDto);

            var previousPageLink = shoppingListItemssFromRepo.HasPrevious
                    ? CreateShoppingListItemsResourceUri(request.ShoppingListItemParametersDto,
                        ResourceUriType.PreviousPage,
                        request.Controller)
                    : null;

            var nextPageLink = shoppingListItemssFromRepo.HasNext
                ? CreateShoppingListItemsResourceUri(request.ShoppingListItemParametersDto,
                    ResourceUriType.NextPage,
                    request.Controller)
                : null;

            var paginationMetadata = new PaginationHeader
            {
                TotalCount = shoppingListItemssFromRepo.TotalCount,
                PageSize = shoppingListItemssFromRepo.PageSize,
                PageNumber = shoppingListItemssFromRepo.PageNumber,
                TotalPages = shoppingListItemssFromRepo.TotalPages,
                HasPrevious = shoppingListItemssFromRepo.HasPrevious,
                HasNext = shoppingListItemssFromRepo.HasNext,
                PreviousPageLink = previousPageLink,
                NextPageLink = nextPageLink
            };

            var returnableShoppingListItem = _mapper.Map<IEnumerable<ShoppingListItemDto>>(shoppingListItemssFromRepo);

            var response = new GetAllShoppingListItemQueryResponse { PaginationMetadata = paginationMetadata, ShoppingListItemDtoList = returnableShoppingListItem };
            return response;
        }

        private string CreateShoppingListItemsResourceUri(
            ShoppingListItemParametersDto shoppingListItemsParametersDto,
            ResourceUriType type,
            Controller controller)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return controller.Url.Link("GetShoppingListItems",
                        new
                        {
                            filters = shoppingListItemsParametersDto.Filters,
                            orderBy = shoppingListItemsParametersDto.SortOrder,
                            pageNumber = shoppingListItemsParametersDto.PageNumber - 1,
                            pageSize = shoppingListItemsParametersDto.PageSize,
                            searchQuery = shoppingListItemsParametersDto.QueryString
                        });
                case ResourceUriType.NextPage:
                    return controller.Url.Link("GetShoppingListItems",
                        new
                        {
                            filters = shoppingListItemsParametersDto.Filters,
                            orderBy = shoppingListItemsParametersDto.SortOrder,
                            pageNumber = shoppingListItemsParametersDto.PageNumber + 1,
                            pageSize = shoppingListItemsParametersDto.PageSize,
                            searchQuery = shoppingListItemsParametersDto.QueryString
                        });

                default:
                    return controller.Url.Link("GetShoppingListItems",
                        new
                        {
                            filters = shoppingListItemsParametersDto.Filters,
                            orderBy = shoppingListItemsParametersDto.SortOrder,
                            pageNumber = shoppingListItemsParametersDto.PageNumber,
                            pageSize = shoppingListItemsParametersDto.PageSize,
                            searchQuery = shoppingListItemsParametersDto.QueryString
                        });
            }
        }
    }
}
