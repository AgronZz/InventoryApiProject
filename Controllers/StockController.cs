using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventoryApiProject.Repositories;
using InventoryApiProject.Dtos;

namespace InventoryApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IStockTransactionRepository _stockRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StockController> _logger;

        public StockController(
            IStockTransactionRepository stockRepository,
            IMapper mapper,
            ILogger<StockController> logger)
        {
            _stockRepository = stockRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET ALL STOCK TRANSACTIONS
        [HttpGet]
        [Authorize(Roles = "Administrator,Staff")]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _stockRepository.GetAllAsync();  //calls the method from repository to get all stock transactions

            return Ok(_mapper.Map<List<StockTransactionDto>>(transactions));  //returns the list of stock transactions mapped to DTOs using AutoMapper
        }

        // GET BY PRODUCT
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(Guid productId)
        {
            if (productId == Guid.Empty)
                return BadRequest();

            var result = await _stockRepository.GetByProductIdAsync(productId);

            return Ok(_mapper.Map<List<StockTransactionDto>>(result));
        }


        [HttpGet("summary")]
        public async Task<IActionResult> Summary()
        {
            var transactions = await _stockRepository.GetAllAsync();  //calls the method from repository to get all stock transactions

            var summary = transactions
                .GroupBy(t => t.ProductId)  //group transactions by product ID to calculate total IN and OUT quantities for each product
                .Select(g => new  //create a new anonymous object for each product group containing the product ID, total IN quantity, and total OUT quantity
                {
                    ProductId = g.Key,  //the product ID is the key of the group
                    TotalIn = g.Where(x => x.Type == "IN").Sum(x => x.QuantityChanged),  //calculate total IN quantity by filtering transactions of type "IN" and summing their QuantityChanged values
                    TotalOut = g.Where(x => x.Type == "OUT").Sum(x => x.QuantityChanged),  //calculate total OUT quantity by filtering transactions of type "OUT" and summing their QuantityChanged values
                });

            _logger.LogInformation("Stock summary generated");

            return Ok(summary);
        }
    }
}