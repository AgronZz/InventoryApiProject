namespace InventoryApiProject.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string SKU { get; set; } //SKU = Stock Keeping Unit, Unique inventory code.

        public decimal Price { get; set; }

        public int QuantityInStock { get; set; }  //Current stock amount

        public Guid CategoryId { get; set; } //foreign key - Which category does this product belong to?
        public Category Category { get; set; }  //navigation property - Many Products belong to one Category

        public Guid SupplierId { get; set; } //foreign key - Which supplier provides this product?
        public Supplier Supplier { get; set; }  //navigation property - Many Products can be supplied by one Supplier

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
