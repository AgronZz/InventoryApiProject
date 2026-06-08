using InventoryApiProject.Models;

namespace InventoryApi.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(Guid id);

        Task<bool> CreateOrderAsync(Order order);

        Task<bool> UpdateStatusAsync(Guid id, string status);

        Task<bool> DeleteAsync(Guid id);

        Task SaveChanges();
    }
}