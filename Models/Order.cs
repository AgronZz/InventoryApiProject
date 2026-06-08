namespace InventoryApiProject.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; } // Pending, Completed

        public List<OrderItem> Items { get; set; }  //One Order contains many OrderItems
    }
}
