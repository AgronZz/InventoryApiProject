namespace InventoryApiProject.Models
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContactEmail { get; set; }

        public List<Product> Products { get; set; }  //One supplier can supply many products.
    }
}
