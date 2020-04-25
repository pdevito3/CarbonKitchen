namespace CarbonKitchen.Ingredients.Api.Services
{
    using CarbonKitchen.Ingredients.Api.Data.Entities;
    using CarbonKitchen.Ingredients.Api.Models;
    using CarbonKitchen.Ingredients.Api.Models.Pagination;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IIngredientRepository
    {
        PagedList<Ingredient> GetIngredients(IngredientParametersDto ingredientParameters);
        Task<Ingredient> GetIngredientAsync(int ingredientId);
        Ingredient GetIngredient(int ingredientId);
        void AddIngredient(Ingredient ingredient);
        void DeleteIngredient(Ingredient ingredient);
        void UpdateIngredient(Ingredient ingredient);
        bool Save();
    }
}
