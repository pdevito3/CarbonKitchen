namespace CarbonKitchen.Ingredients.Api.Services
{
    using CarbonKitchen.Ingredients.Api.Data.Entities;
    using CarbonKitchen.Ingredients.Api.Models;
    using CarbonKitchen.Ingredients.Api.Models.Pagination;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        void AddIngredients(List<Ingredient> ingredient);
        void DeleteIngredient(Ingredient ingredient);

        //TODO: Update to only use this, not the list
        void DeleteIngredients(int recipeId); // this wasn't working, but will leave it as a todo as it is better practice 
        void DeleteIngredients(List<Ingredient> ingredients);
        void UpdateIngredient(Ingredient ingredient);
        bool Save();
    }
}
