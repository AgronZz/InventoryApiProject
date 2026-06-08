namespace InventoryApiProject.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }  //foreign key
        public Product Product { get; set; }

        public int Quantity { get; set; } //How many units were ordered
    }
}
