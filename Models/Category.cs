namespace InventoryApiProject.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; set; }  //One Category has many Products
    }
}
