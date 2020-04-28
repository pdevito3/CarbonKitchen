using CarbonKitchen.Ingredients.Api.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonKitchen.Ingredients.Api.Models
{
    public class IngredientParametersDto : IngredientPaginationParameters
    {
        public string Filters { get; set; }
        public string QueryString { get; set; }
        public string SortOrder { get; set; }
        public int? RecipeId { get; set; }
    }
}
