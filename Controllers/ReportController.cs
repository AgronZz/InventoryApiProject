using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventoryApiProject.Repositories;
using InventoryApiProject.Dtos;

namespace InventoryApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IStockTransactionRepository _stockRepository;
        private readonly ILogger<ReportController> _logger;

        public ReportController(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IStockTransactionRepository stockRepository,
            ILogger<ReportController> logger)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _stockRepository = stockRepository;
            _logger = logger;
        }

        // ============================================
        // 1. LOW STOCK PRODUCTS
        // ============================================
        [HttpGet("low-stock")]
        [Authorize(Roles = "Administrator")]
        //Returns products whose stock is below a threshold
        public async Task<IActionResult> LowStock(int threshold = 10)
        {
            var products = await _productRepository.GetAllAsync();  //calls the method from repository to get all products

            var result = products
                .Where(p => p.QuantityInStock <= threshold)  //filter products where quantity in stock is less than or equal to the specified threshold
                .Select(p => new  //create a new anonymous object for each low stock product containing the product ID, name, quantity in stock, and category name
                {
                    p.Id,
                    p.Name,
                    p.QuantityInStock,
                    Category = p.Category.Name
                });

            _logger.LogInformation("Generated low stock report");

            return Ok(result);
        }

        // ============================================
        // 2. TOTAL INVENTORY VALUE
        // ============================================
        [HttpGet("inventory-value")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> TotalInventoryValue()
        {
            var products = await _productRepository.GetAllAsync();  //calls the method from repository to get all products

            var totalValue = products.Sum(p =>  //calculate total inventory value by summing the product of price and quantity in stock for each product
                p.Price * p.QuantityInStock);

            _logger.LogInformation("Generated inventory value report");

            return Ok(new
            {
                TotalInventoryValue = totalValue
            });
        }

        // ============================================
        // 3. PRODUCTS GROUPED BY CATEGORY
        // ============================================
        [HttpGet("products-by-category")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ProductsByCategory()
        {
            var products = await _productRepository.GetAllAsync();  //calls the method from repository to get all products

            //var result = products
            //    .GroupBy(p => p.Category.Name)
            //    .Select(g => new
            //    {
            //        Category = g.Key,
            //        ProductCount = g.Count()
            //    });

            var result = products
                .GroupBy(p => p.Category.Name)  //group products by their category name to count how many products belong to each category
                .Select(g => new ProductsByCategoryDto  //create a new ProductsByCategoryDto object for each category group containing the category name and the count of products in that category
                {
                    Category = g.Key,
                    ProductCount = g.Count()
                });

            return Ok(result);
        }

        // ============================================
        // 4. ORDER STATUS REPORT
        // ============================================
        [HttpGet("orders-by-status")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> OrdersByStatus()
        {
            var orders = await _orderRepository.GetAllAsync();  //calls the method from repository to get all orders

            var result = orders
                .GroupBy(o => o.Status)  //group orders by their status to count how many orders are in each status category (e.g., Pending, Shipped, Delivered)
                .Select(g => new  //create a new anonymous object for each status group containing the status and the count of orders in that status
                {
                    Status = g.Key,
                    Count = g.Count()
                });

            return Ok(result);
        }

        // ============================================
        // 5. MOST SOLD PRODUCTS
        // ============================================
        [HttpGet("top-products")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> TopProducts()
        {
            var orders = await _orderRepository.GetAllAsync();  //calls the method from repository to get all orders, which will include the order items and their associated products

            var result = orders
                .SelectMany(o => o.Items)  //flatten the list of orders into a list of order items to analyze which products were sold and in what quantities
                .GroupBy(i => i.Product.Name)  //group order items by the name of the product to calculate total quantity sold for each product
                .Select(g => new
                {
                    Product = g.Key,
                    TotalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)  //sort the products in descending order based on the total quantity sold to identify the most popular products
                .Take(10);  //take the top 10 most sold products to return in the report

            return Ok(result);
        }

        // ============================================
        // 6. STOCK MOVEMENT REPORT
        // ============================================
        [HttpGet("stock-movement")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> StockMovement()
        {
            var transactions = await _stockRepository.GetAllAsync();  //calls the method from repository to get all stock transactions, which will include both IN and OUT transactions for each product

            var result = transactions
                .GroupBy(t => t.Product.Name)  //group stock transactions by the name of the product to calculate total IN and OUT quantities for each product
                .Select(g => new   //create a new anonymous object for each product group containing the product name, total IN quantity, and total OUT quantity
                {
                    Product = g.Key,

                    TotalIn = g
                        .Where(x => x.Type == "IN")
                        .Sum(x => x.QuantityChanged),

                    TotalOut = g
                        .Where(x => x.Type == "OUT")
                        .Sum(x => Math.Abs(x.QuantityChanged))
                });

            return Ok(result);
        }
    }
}