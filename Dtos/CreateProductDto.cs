namespace InventoryApi.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }

        public Guid CategoryId { get; set; }
        public Guid SupplierId { get; set; }
    }
}
