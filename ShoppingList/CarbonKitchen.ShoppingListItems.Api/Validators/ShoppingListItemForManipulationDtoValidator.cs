namespace CarbonKitchen.ShoppingListItems.Api.Validators
{
    using FluentValidation;
    using CarbonKitchen.ShoppingListItems.Api.Models;
    using System;

    public class ShoppingListItemForManipulationDtoValidator<T> : AbstractValidator<T> where T : ShoppingListItemForManipulationDto
    {
        public ShoppingListItemForManipulationDtoValidator()
        {
        }
    }
}
