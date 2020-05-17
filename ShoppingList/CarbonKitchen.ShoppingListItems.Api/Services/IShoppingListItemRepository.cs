namespace CarbonKitchen.ShoppingListItems.Api.Services
{
    using CarbonKitchen.ShoppingListItems.Api.Data.Entities;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Models.Pagination;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IShoppingListItemRepository
    {
        PagedList<ShoppingListItem> GetShoppingListItems(ShoppingListItemParametersDto shoppingListItemsParameters);
        Task<ShoppingListItem> GetShoppingListItemAsync(int shoppingListItemsId);
        ShoppingListItem GetShoppingListItem(int shoppingListItemsId);
        void AddShoppingListItem(ShoppingListItem shoppingListItems);
        void DeleteShoppingListItem(ShoppingListItem shoppingListItems);
        void UpdateShoppingListItem(ShoppingListItem shoppingListItems);
        bool Save();
    }
}
