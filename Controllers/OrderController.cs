using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using InventoryApiProject.Models;
using InventoryApiProject.Repositories;
using InventoryApiProject.Dtos;
using InventoryApiProject.Dtos;
using InventoryApiProject.Models;
using InventoryApi.Repositories;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<OrderController> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // ================================
        // 1. GET ALL ORDERS
        // ================================
        [HttpGet]
        [Authorize(Roles = "Administrator,Staff")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepository.GetAllAsync();  //call the method from repository to get all orders

            var result = _mapper.Map<List<OrderDto>>(orders);  //convert order entities to order DTOs using AutoMapper

            _logger.LogInformation("Fetched all orders");

            return Ok(result);
        }

        // ================================
        // 2. GET ORDER BY ID
        // ================================
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Staff")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid order ID");

            var order = await _orderRepository.GetByIdAsync(id);  //calls the method from repository to get order by ID

            if (order == null)  //if order is not found, log a warning and return 404 Not Found
            {
                _logger.LogWarning("Order not found: {OrderId}", id);
                return NotFound();
            }

            var result = _mapper.Map<OrderDto>(order);  //map the order entity to an order DTO using AutoMapper

            return Ok(result);
        }

        // ================================
        // 3. CREATE ORDER (CORE LOGIC)
        // ================================
        [HttpPost]
        [Authorize(Roles = "Administrator,Staff")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
                return BadRequest("Order must contain items");

            var order = _mapper.Map<Order>(dto);  //Convert the incoming DTO to an Order entity using AutoMapper

            //order.Id = Guid.NewGuid();
            //order.OrderDate = DateTime.UtcNow;
            //order.Status = "Pending";

            await _orderRepository.CreateOrderAsync(order);  //calls the method from repository to create the order, which will handle the business logic of validating stock and calculating total price
            await _orderRepository.SaveChanges();  //save the changes

            _logger.LogInformation("Order created: {OrderId}", order.Id);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        // ================================
        // 4. UPDATE ORDER STATUS
        // ================================
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var result = await _orderRepository.UpdateStatusAsync(id, status);  //call the method from repo

            if (!result)
                return NotFound();  //if the order is not found, return 404 Not Found

            await _orderRepository.SaveChanges();  //save the changes

            _logger.LogInformation("Order status updated: {OrderId} -> {Status}", id, status);

            return NoContent();
        }

        // ================================
        // 5. DELETE ORDER
        // ================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _orderRepository.DeleteAsync(id);

            if (!result)
                return NotFound();

            await _orderRepository.SaveChanges();

            _logger.LogWarning("Order deleted: {OrderId}", id);

            return NoContent();
        }


    }
}