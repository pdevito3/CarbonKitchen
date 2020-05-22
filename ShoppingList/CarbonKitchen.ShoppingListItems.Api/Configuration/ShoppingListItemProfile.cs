namespace CarbonKitchen.ShoppingListItems.Api.Configuration
{
    using AutoMapper;
    using CarbonKitchen.ShoppingListItems.Api.Data.Entities;
    using CarbonKitchen.ShoppingListItems.Api.Models;

    public class ShoppingListItemProfile : Profile
    {
        public ShoppingListItemProfile()
        {
            //createmap<to this, from this>
            CreateMap<ShoppingListItem, ShoppingListItemDto>()
                .ReverseMap();
            CreateMap<ShoppingListItemForCreationDto, ShoppingListItem>();
            CreateMap<ShoppingListItemForUpdateDto, ShoppingListItem>()
                .ReverseMap();
        }
    }
}