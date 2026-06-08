
using InventoryApiProject.Models;

namespace InventoryApiProject.Repositories
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier> GetByIdAsync(Guid id);

        Task<bool> AddAsync(Supplier supplier);
        Task<bool> UpdateAsync(Guid id, Supplier supplier);
        Task<bool> DeleteAsync(Guid id);

        Task SaveChanges();
    }
}