namespace InventoryApiProject.Dtos
{
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContactEmail { get; set; }
        public int ProductCount { get; set; }  //Number of products supplied
    }
}
