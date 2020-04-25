namespace CarbonKitchen.Ingredients.Api.Configuration
{
    using AutoMapper;
    using CarbonKitchen.Ingredients.Api.Data.Entities;
    using CarbonKitchen.Ingredients.Api.Models;

    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            //createmap<to this, from this>
            CreateMap<Ingredient, IngredientDto>()
                .ReverseMap();
            CreateMap<IngredientForCreationDto, Ingredient>();
            CreateMap<IngredientForUpdateDto, Ingredient>()
                .ReverseMap();
        }
    }
}