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

        public PagedList<ShoppingListItem> GetShoppingListItems(ShoppingListItemParametersDto shoppingListItemParameters)
        {
            if (shoppingListItemParameters == null)
            {
                throw new ArgumentNullException(nameof(shoppingListItemParameters));
            }

            var collection = _context.ShoppingListItems as IQueryable<ShoppingListItem>;

            if (!string.IsNullOrWhiteSpace(shoppingListItemParameters.QueryString))
            {
                var QueryString = shoppingListItemParameters.QueryString.Trim();
                collection = collection.Where(sli => sli.Name.Contains(QueryString)
                    || sli.Category.Contains(QueryString));
            }

            var sieveModel = new SieveModel
            {
                Sorts = shoppingListItemParameters.SortOrder,
                Filters = shoppingListItemParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return PagedList<ShoppingListItem>.Create(collection,
                shoppingListItemParameters.PageNumber,
                shoppingListItemParameters.PageSize);
        }

        public async Task<ShoppingListItem> GetShoppingListItemAsync(int shoppingListItemId)
        {
            return await _context.ShoppingListItems.FirstOrDefaultAsync(sli => sli.ShoppingListItemId == shoppingListItemId);
        }

        public ShoppingListItem GetShoppingListItem(int shoppingListItemId)
        {
            return _context.ShoppingListItems.FirstOrDefault(sli => sli.ShoppingListItemId == shoppingListItemId);
        }

        public void AddShoppingListItem(ShoppingListItem shoppingListItem)
        {
            if (shoppingListItem == null)
            {
                throw new ArgumentNullException(nameof(shoppingListItem));
            }

            _context.ShoppingListItems.Add(shoppingListItem);
        }

        public void DeleteShoppingListItem(ShoppingListItem shoppingListItem)
        {
            if (shoppingListItem == null)
            {
                throw new ArgumentNullException(nameof(shoppingListItem));
            }

            _context.ShoppingListItems.Remove(shoppingListItem);
        }

        public void UpdateShoppingListItem(ShoppingListItem shoppingListItem)
        {
            // no implementation for now
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
