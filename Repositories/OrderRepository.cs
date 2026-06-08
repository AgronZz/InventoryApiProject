using Microsoft.EntityFrameworkCore;
using InventoryApiProject.Data;
using InventoryApiProject.Models;

using System;

namespace InventoryApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)  //Load order items
                .ThenInclude(i => i.Product)  //Load product inside each order item
                .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        //Load product inside each order item
        public async Task<bool> CreateOrderAsync(Order order)
        {
            order.OrderDate = DateTime.UtcNow;  //set the date
            order.Status = "Pending";   //set the status

            // loop through each order item, check if product exists and has enough stock, then reduce stock
            foreach (var item in order.Items)  //Process every ordered product
            {
                var product = await _context.Products.FindAsync(item.ProductId);  //Find product

                if (product == null)
                    return false;  //If product doesn't exist order fails

                if (product.QuantityInStock < item.Quantity)
                    return false;  //if not enough stock order fails

                product.QuantityInStock -= item.Quantity;  //Reduce Stock

                // stock stransaction for this order item
                _context.StockTransactions.Add(new StockTransaction  //Create audit record
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    QuantityChanged = -item.Quantity,
                    Type = "OUT",
                    Date = DateTime.UtcNow
                });
            }

            _context.Orders.Add(order);  //store order
            return true;
        }

        public async Task<bool> UpdateStatusAsync(Guid id, string status)
        {
            var order = await _context.Orders.FindAsync(id);  //find order

            if (order == null)
                return false;

            order.Status = status;  //update status
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return false;

            _context.Orders.Remove(order);
            return true;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}