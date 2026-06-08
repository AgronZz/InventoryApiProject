namespace InventoryApiProject.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ProductCount { get; set; }  //ProductCount doesn't exist in Category table, it's calculated in the service layer by counting the number of products associated with this category.
    }
}
