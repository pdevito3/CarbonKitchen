namespace CarbonKitchen.ShoppingListItems.Api.Models
{
    using System;

    public class ShoppingListItemDto
    {
        public int ShoppingListItemId { get; set; }
        public int? Amount { get; set; }
        public string Name { get; set; }
        public int? ShoppingListId { get; set; }
        public bool? Acquired { get; set; }
        public bool? Hidden { get; set; }
    }
}
