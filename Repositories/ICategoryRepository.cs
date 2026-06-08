
using InventoryApiProject.Models;

namespace InventoryApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(Guid id);

        Task<bool> AddAsync(Category category);
        Task<bool> UpdateAsync(Guid id, Category category);
        Task<bool> DeleteAsync(Guid id);

        Task SaveChanges();   //Persist changes to database
    }
}