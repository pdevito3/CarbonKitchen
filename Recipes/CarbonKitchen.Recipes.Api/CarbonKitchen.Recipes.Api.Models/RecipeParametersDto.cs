using CarbonKitchen.Recipes.Api.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonKitchen.Recipes.Api.Models
{
    public class RecipeParametersDto : RecipePaginationParameters
    {
        public string Filters { get; set; }
        public string QueryString { get; set; }
        public string SortOrder { get; set; }
    }
}
