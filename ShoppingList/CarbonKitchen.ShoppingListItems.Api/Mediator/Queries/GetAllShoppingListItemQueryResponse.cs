using CarbonKitchen.ShoppingListItems.Api.Models;
using CarbonKitchen.ShoppingListItems.Api.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarbonKitchen.ShoppingListItems.Api.Mediator.Queries
{
    public class GetAllShoppingListItemQueryResponse
    {
        public IEnumerable<ShoppingListItemDto> ShoppingListItemDtoList { get; set; }
        public PaginationHeader PaginationMetadata { get; set; }
    }
}
