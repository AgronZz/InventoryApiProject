namespace InventoryApiProject.Models
{
    public class StockTransaction
    {
        //StockTransaction → Inventory movements(IN/OUT)
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }  //Which product changed?
        public Product Product { get; set; }

        public int QuantityChanged { get; set; } // + or -  , Stores quantity change

        public string Type { get; set; } // IN / OUT

        public DateTime Date { get; set; }

    }
}
