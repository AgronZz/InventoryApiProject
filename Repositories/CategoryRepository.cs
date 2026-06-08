//using Microsoft.EntityFrameworkCore;
//using InventoryApiProject.Data;
//using InventoryApiProject.Models;

//namespace InventoryApi.Repositories
//{
//    public class CategoryRepository : ICategoryRepository
//    {
//        private readonly AppDbContext _context;

//        public CategoryRepository(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<Category>> GetAllAsync()
//        {
//            return await _context.Categories
//                .Include(c => c.Products)
//                .ToListAsync();
//        }

//        public async Task<Category> GetByIdAsync(Guid id)
//        {
//            return await _context.Categories
//                .Include(c => c.Products)
//                .FirstOrDefaultAsync(c => c.Id == id);
//        }

//        public async Task<bool> AddAsync(Category category)
//        {
//            await _context.Categories.AddAsync(category);
//            return true;
//        }

//        public async Task<bool> UpdateAsync(Guid id, Category category)
//        {
//            var existing = await _context.Categories.FindAsync(id);

//            if (existing == null)
//                return false;

//            existing.Name = category.Name;

//            return true;
//        }

//        public async Task<bool> DeleteAsync(Guid id)
//        {
//            var existing = await _context.Categories.FindAsync(id);

//            if (existing == null)
//                return false;

//            _context.Categories.Remove(existing);
//            return true;
//        }

//        public async Task SaveChanges()
//        {
//            await _context.SaveChangesAsync();
//        }
//    }
//}


using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApiPRoject.Models;

namespace InventoryApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;  //Stores database context

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories   //Start querying Categories table
                .Include(c => c.Products)  //Loads related products
                .ToListAsync();
            //without Include it will load only Category data, with include it will also load related products for each category
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            //return Category if found, if not then null
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> AddAsync(Category category)
        {
            _context.Categories.Add(category);  //Marks category as Added
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, Category category)
        {
            var existing = await _context.Categories.FindAsync(id);  //finds the id first

            if (existing == null)
                return false;

            existing.Name = category.Name;  //update the property, EF automatically tracks this change

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Categories.FindAsync(id);

            if (existing == null)
                return false;

            _context.Categories.Remove(existing);
            return true;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}