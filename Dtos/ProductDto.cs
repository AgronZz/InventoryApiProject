namespace InventoryApiProject.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }

        public string CategoryName { get; set; }  //return category name instead of category id because user want to see category name instead of category id when get product list or product details
        public string SupplierName { get; set; }  //return supplier name instead of supplier id
    }
}
