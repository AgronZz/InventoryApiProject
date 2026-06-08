namespace InventoryApiProject.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public List<OrderItemDto> Items { get; set; }
    }
}
