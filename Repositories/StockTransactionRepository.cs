using Microsoft.EntityFrameworkCore;
using InventoryApiProject.Data;
using InventoryApiProject.Models;

namespace InventoryApiProject.Repositories
{
    public class StockTransactionRepository : IStockTransactionRepository
    {
        private readonly AppDbContext _context;

        public StockTransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockTransaction>> GetAllAsync()
        {
            //Returns all stock movements
            return await _context.StockTransactions
                .Include(t => t.Product)  //Load product information
                .ToListAsync();
        }

        public async Task<IEnumerable<StockTransaction>> GetByProductIdAsync(Guid productId)
        {
            return await _context.StockTransactions
                .Where(t => t.ProductId == productId)   //Filter transactions
                .Include(t => t.Product)
                .ToListAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}