namespace CarbonKitchen.ShoppingListItems.Api.Services
{
    using CarbonKitchen.ShoppingListItems.Api.Data;
    using CarbonKitchen.ShoppingListItems.Api.Data.Entities;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using CarbonKitchen.ShoppingListItems.Api.Models.Pagination;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ShoppingListItemRepository : IShoppingListItemRepository
    {
        private ShoppingListItemDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public ShoppingListItemRepository(ShoppingListItemDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public PagedList<ShoppingListItem> GetShoppingListItems(ShoppingListItemParametersDto shoppingListItemsParameters)
        {
            if (shoppingListItemsParameters == null)
            {
                throw new ArgumentNullException(nameof(shoppingListItemsParameters));
            }

            var collection = _context.ShoppingListItems as IQueryable<ShoppingListItem>;

            if (!string.IsNullOrWhiteSpace(shoppingListItemsParameters.QueryString))
            {
                var QueryString = shoppingListItemsParameters.QueryString.Trim();
                collection = collection.Where(sli => sli.Name.Contains(QueryString));
            }

            var sieveModel = new SieveModel
            {
                Sorts = shoppingListItemsParameters.SortOrder,
                Filters = shoppingListItemsParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return PagedList<ShoppingListItem>.Create(collection,
                shoppingListItemsParameters.PageNumber,
                shoppingListItemsParameters.PageSize);
        }

        public async Task<ShoppingListItem> GetShoppingListItemAsync(int shoppingListItemsId)
        {
            return await _context.ShoppingListItems.FirstOrDefaultAsync(sli => sli.ShoppingListItemId == shoppingListItemsId);
        }

        public ShoppingListItem GetShoppingListItem(int shoppingListItemsId)
        {
            return _context.ShoppingListItems.FirstOrDefault(sli => sli.ShoppingListItemId == shoppingListItemsId);
        }

        public void AddShoppingListItem(ShoppingListItem shoppingListItems)
        {
            if (shoppingListItems == null)
            {
                throw new ArgumentNullException(nameof(shoppingListItems));
            }

            _context.ShoppingListItems.Add(shoppingListItems);
        }

        public void DeleteShoppingListItem(ShoppingListItem shoppingListItems)
        {
            if (shoppingListItems == null)
            {
                throw new ArgumentNullException(nameof(shoppingListItems));
            }

            _context.ShoppingListItems.Remove(shoppingListItems);
        }

        public void UpdateShoppingListItem(ShoppingListItem shoppingListItems)
        {
            // no implementation for now
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
