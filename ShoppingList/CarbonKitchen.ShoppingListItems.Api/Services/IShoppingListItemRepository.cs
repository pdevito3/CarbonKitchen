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
        PagedList<ShoppingListItem> GetShoppingListItems(ShoppingListItemParametersDto shoppingListItemParameters);
        Task<ShoppingListItem> GetShoppingListItemAsync(int shoppingListItemId);
        ShoppingListItem GetShoppingListItem(int shoppingListItemId);
        void AddShoppingListItem(ShoppingListItem shoppingListItem);
        void DeleteShoppingListItem(ShoppingListItem shoppingListItem);
        void UpdateShoppingListItem(ShoppingListItem shoppingListItem);
        bool Save();
    }
}
