namespace CarbonKitchen.Ingredients.Api.Data
{
    using CarbonKitchen.Ingredients.Api.Data.Entities;
    using Microsoft.EntityFrameworkCore;

    public class IngredientDbContext : DbContext
    {
        public IngredientDbContext(DbContextOptions<IngredientDbContext> options) : base(options) { }

        public DbSet<Ingredient> Ingredients { get; set; }
    }
}
