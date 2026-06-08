namespace InventoryApiProject.Dtos
{
    public class CreateOrderDto
    {
        public List<CreateOrderItemDto> Items { get; set; }
    }
}
