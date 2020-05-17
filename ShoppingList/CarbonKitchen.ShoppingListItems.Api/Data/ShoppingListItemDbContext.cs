namespace CarbonKitchen.ShoppingListItems.Api.Data
{
    using CarbonKitchen.ShoppingListItems.Api.Data.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ShoppingListItemDbContext : DbContext
    {
        public ShoppingListItemDbContext(DbContextOptions<ShoppingListItemDbContext> options) : base(options) { }

        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
    }
}
