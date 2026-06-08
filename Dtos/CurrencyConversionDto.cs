namespace InventoryApiProject.Dtos
{
    public class CurrencyConversionDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }
        public decimal Result { get; set; }
    }
}
