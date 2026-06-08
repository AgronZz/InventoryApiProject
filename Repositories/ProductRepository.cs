

using Microsoft.EntityFrameworkCore;
using InventoryApiProject.Data;
using InventoryApiProject.Models;

namespace InventoryApiProject.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)  //Load category
                .Include(p => p.Supplier) //Load supplier
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> AddAsync(Product product)
        {
            _context.Products.Add(product);  //Marks entity for insertion
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, Product product)
        {
            var existing = await _context.Products.FindAsync(id);  //find the product

            if (existing == null)
                return false;

            //update the fields of name, sku, price, stock,quantity in stock, category and supplier
            existing.Name = product.Name;
            existing.SKU = product.SKU;
            existing.Price = product.Price;
            existing.QuantityInStock = product.QuantityInStock;
            existing.CategoryId = product.CategoryId;
            existing.SupplierId = product.SupplierId;
            existing.UpdatedAt = DateTime.UtcNow;

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Products.FindAsync(id);

            if (existing == null)
                return false;

            _context.Products.Remove(existing);
            return true;
        }

        //implements the search method with pagination, filtering and sorting
        public async Task<List<Product>> Search(
            int pageNumber,
            int pageSize,
            Guid? categoryId,
            Guid? supplierId,
            decimal? priceFrom,
            decimal? priceTo,
            string? searchText,
            string? sortField,
            bool? sortOrder)
        {
            var query = _context.Products  //Start with all products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsQueryable();  //Allows query construction step by step, Nothing executes yet.

            if (categoryId != null)  //filter by category if provided
                query = query.Where(p => p.CategoryId == categoryId);

            if (supplierId != null)  //filter by supplier if provided
                query = query.Where(p => p.SupplierId == supplierId);

            if (priceFrom != null)  //filter by minimum price if provided
                query = query.Where(p => p.Price >= priceFrom);

            if (priceTo != null)  //filter by maximum price if provided
                query = query.Where(p => p.Price <= priceTo);

            if (!string.IsNullOrEmpty(searchText))  //search by name if search text is provided
                query = query.Where(p => p.Name.Contains(searchText));

            switch (sortField)
            {
                case "Name":
                    query = sortOrder == true ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);  //sort by name ascending or descending based on sortOrder
                    break;

                case "Price":
                    query = sortOrder == true ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);  //sort by price ascending or descending based on sortOrder
                    break;

                case "Stock":
                    query = sortOrder == true ? query.OrderBy(p => p.QuantityInStock) : query.OrderByDescending(p => p.QuantityInStock);  //sort by stock ascending or descending based on sortOrder
                    break;

                default:
                    query = query.OrderBy(p => p.Name); //default sort by name ascending if no sort field is provided
                    break;
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)  //Skip the products of previous pages based on page number and page size
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> TotalCountProducts(
            Guid? categoryId,
            Guid? supplierId,
            decimal? priceFrom,
            decimal? priceTo,
            string? searchText)
        {
            var query = _context.Products.AsQueryable();

            if (categoryId != null)
                query = query.Where(p => p.CategoryId == categoryId);

            if (supplierId != null)
                query = query.Where(p => p.SupplierId == supplierId);

            if (priceFrom != null)
                query = query.Where(p => p.Price >= priceFrom);

            if (priceTo != null)
                query = query.Where(p => p.Price <= priceTo);

            if (!string.IsNullOrEmpty(searchText))
                query = query.Where(p => p.Name.Contains(searchText));

            return await query.CountAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}