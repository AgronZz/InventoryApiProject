

using InventoryApiProject.Models;

namespace InventoryApiProject.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(Guid id);

        Task<List<Product>> Search(
            int pageNumber,
            int pageSize,
            Guid? categoryId,
            Guid? supplierId,
            decimal? priceFrom,
            decimal? priceTo,
            string? searchText,
            string? sortField,
            bool? sortOrder);

        Task<int> TotalCountProducts(
            Guid? categoryId,
            Guid? supplierId,
            decimal? priceFrom,
            decimal? priceTo,
            string? searchText);

        Task<bool> AddAsync(Product product);
        Task<bool> UpdateAsync(Guid id, Product product);
        Task<bool> DeleteAsync(Guid id);

        Task SaveChanges();
    }
}