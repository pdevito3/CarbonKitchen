namespace CarbonKitchen.ShoppingListItems.Api.Models.Pagination
{
    public abstract class ShoppingListItemPaginationParameters
    {
        const int maxPageSize = 200;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 200;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
