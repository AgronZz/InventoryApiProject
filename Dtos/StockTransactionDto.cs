namespace InventoryApiProject.Dtos
{
    public class StockTransactionDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }

        public int QuantityChanged { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }

    }
}
