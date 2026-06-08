using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventoryApiProject.Dtos;
using InventoryApiProject.Models;
using InventoryApiProject.Repositories;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // =========================
        // GET ALL
        // =========================
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetAll()
        //{
        //    var products = await _productRepository.GetAllAsync();

        //    _logger.LogInformation("Fetched all products");

        //    var result = _mapper.Map<List<ProductDto>>(products);
        //    return Ok(result);
        //}
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var products = await _productRepository.GetAllAsync();  //Repository returns all products

            var total = products.Count();  //calculate total number of products for pagination metadata

            //apply pagination
            var paged = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            //response is formatted as a PagedModel which includes the paginated items and metadata about total items, page number, page size, and total pages
            var result = new PagedModel<ProductDto>
            {
                Items = _mapper.Map<List<ProductDto>>(paged),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            };

            return Ok(result);
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        // =========================
        // PAGINATED SEARCH
        // =========================
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            int pageNumber = 1,
            int pageSize = 10,
            Guid? categoryId = null,
            Guid? supplierId = null,
            decimal? priceFrom = null,
            decimal? priceTo = null,
            string? searchText = null,
            string? sortField = null,
            bool? sortOrder = true)
        {
            //calls method Search from repo which applies the filtering, sorting, and pagination logic based on the provided parameters. The result is a list of products that match the search criteria for the specified page.
            var products = await _productRepository.Search(
                pageNumber,
                pageSize,
                categoryId,
                supplierId,
                priceFrom,
                priceTo,
                searchText,
                sortField,
                sortOrder
            );

            var total = await _productRepository.TotalCountProducts(
                categoryId,
                supplierId,
                priceFrom,
                priceTo,
                searchText
            );

            //return Ok(new
            //{
            //    Data = _mapper.Map<List<ProductDto>>(products),
            //    TotalItems = total,
            //    PageNumber = pageNumber,
            //    PageSize = pageSize
            //});

            //returns paginated result with metadata about total items, page number, page size, and total pages. This structure is useful for clients to understand the pagination context of the results.
            var result = new PagedModel<ProductDto>
            {
                Items = _mapper.Map<List<ProductDto>>(products),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            };

            return Ok(result);
        }

        // =========================
        // CREATE PRODUCT
        // =========================
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            if (dto == null)
                return BadRequest();

            var product = _mapper.Map<Product>(dto);  //convert dto

            await _productRepository.AddAsync(product);  //add to repo
            await _productRepository.SaveChanges(); //save changes

            _logger.LogInformation($"Product created: {product.Name}");

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // =========================
        // UPDATE PRODUCT
        // =========================
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
        {
            if (id == Guid.Empty || dto == null)
                return BadRequest();

            var product = _mapper.Map<Product>(dto);  //convert dto to product model

            var result = await _productRepository.UpdateAsync(id, product);  //call the method from repo to update the product with the given id using the data from the product model

            if (!result)
                return NotFound();

            await _productRepository.SaveChanges();

            _logger.LogInformation($"Product updated: {id}");

            return NoContent();
        }

        // =========================
        // DELETE PRODUCT
        // =========================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productRepository.DeleteAsync(id);

            if (!result)
                return NotFound();

            await _productRepository.SaveChanges();

            _logger.LogWarning($"Product deleted: {id}");

            return NoContent();
        }
    }
}