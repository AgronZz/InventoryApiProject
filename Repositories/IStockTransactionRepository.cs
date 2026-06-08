using InventoryApiProject.Models;

namespace InventoryApiProject.Repositories
{
    public interface IStockTransactionRepository
    {
        Task<IEnumerable<StockTransaction>> GetAllAsync();
        Task<IEnumerable<StockTransaction>> GetByProductIdAsync(Guid productId);

        Task SaveChanges();
    }
}