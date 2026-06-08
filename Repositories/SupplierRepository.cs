
using Microsoft.EntityFrameworkCore;
using InventoryApiProject.Data;
using InventoryApiProject.Models;

namespace InventoryApiProject.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            //Loads products supplied by supplier
            return await _context.Suppliers
                .Include(s => s.Products)
                .ToListAsync();
        }

        public async Task<Supplier> GetByIdAsync(Guid id)
        {
            return await _context.Suppliers
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> AddAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, Supplier supplier)
        {
            var existing = await _context.Suppliers.FindAsync(id);  //first find the existing supplier by id

            if (existing == null)
                return false;

            existing.Name = supplier.Name;  //Update supplier name
            existing.ContactEmail = supplier.ContactEmail;  //Update supplier contact email

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Suppliers.FindAsync(id);

            if (existing == null)
                return false;

            _context.Suppliers.Remove(existing);
            return true;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}