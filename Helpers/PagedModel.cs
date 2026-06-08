namespace InventoryApiProject.Helpers
{
    public class PagedModel<T>
    {
        public int PageNumber { get; set; } //current page, example: 2
        public int PageSize { get; set; }  //Items per page, example: 10
        public int TotalItems { get; set; }  //Total records, example: 125
        public int TotalPages { get; set; } //example: 13 because 125 / 10 = 12.5
        public List<T> Items { get; set; } = new();
    }
}
