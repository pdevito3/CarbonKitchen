using CarbonKitchen.ShoppingListItems.Api.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonKitchen.ShoppingListItems.Api.Models
{
    public class ShoppingListItemParametersDto : ShoppingListItemPaginationParameters
    {
        public string Filters { get; set; }
        public string QueryString { get; set; }
        public string SortOrder { get; set; }
    }
}
