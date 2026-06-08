using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using InventoryApiProject.Models;
using InventoryApiProject.Repositories;
using InventoryApiProject.Dtos;

namespace InventoryApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(
            ISupplierRepository supplierRepository,
            IMapper mapper,
            ILogger<SupplierController> logger)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // ==================================
        // GET ALL SUPPLIERS
        // ==================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliers = await _supplierRepository.GetAllAsync();  //calls the method from repository to get all suppliers

            var result = _mapper.Map<List<SupplierDto>>(suppliers);  //convert supplier entities to supplier DTOs using AutoMapper

            _logger.LogInformation("Fetched all suppliers");

            return Ok(result);  //returns the list of suppliers mapped to DTOs using AutoMapper
        }

        // ==================================
        // GET SUPPLIER BY ID
        // ==================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var supplier = await _supplierRepository.GetByIdAsync(id);  //calls the method from repository to get supplier by ID

            if (supplier == null)  //if supplier is not found, log a warning and return 404 Not Found
                return NotFound();

            return Ok(_mapper.Map<SupplierDto>(supplier));  //returns the supplier mapped to a DTO using AutoMapper
        }

        // ==================================
        // CREATE SUPPLIER
        // ==================================
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateSupplierDto dto)
        {
            if (dto == null)  //if the incoming DTO is null, return a 400 Bad Request response to indicate that the request was invalid
                return BadRequest();

            var supplier = _mapper.Map<Supplier>(dto);  //convert the incoming CreateSupplierDto to a Supplier entity using AutoMapper. This allows us to easily transform the data from the DTO format to the entity format that our repository expects.

            //supplier.Id = Guid.NewGuid();

            await _supplierRepository.AddAsync(supplier);  //add the new supplier entity to the repository using the AddAsync method. This method is asynchronous and will add the supplier to the underlying data store (e.g., database) without blocking the thread.
            await _supplierRepository.SaveChanges();  //save the changes to the repository using the SaveChanges method. This will persist the new supplier to the data store.

            _logger.LogInformation("Supplier created: {Name}", supplier.Name);

            return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);  //returns a 201 Created response with a Location header pointing to the newly created supplier's URL (using the GetById action) and includes the created supplier entity in the response body.
        }

        // ==================================
        // UPDATE SUPPLIER
        // ==================================
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSupplierDto dto)
        {
            if (dto == null)
                return BadRequest();

            var supplier = _mapper.Map<Supplier>(dto);  //convert the incoming UpdateSupplierDto to a Supplier entity using AutoMapper. This allows us to easily transform the data from the DTO format to the entity format that our repository expects.

            var result = await _supplierRepository.UpdateAsync(id, supplier);  //call the UpdateAsync method of the repository to update the supplier with the given ID using the data from the supplier entity. This method will return a boolean indicating whether the update was successful (i.e., if a supplier with the given ID was found and updated).

            if (!result)
                return NotFound();  //if the update was not successful (e.g., if no supplier with the given ID was found), return a 404 Not Found response.

            await _supplierRepository.SaveChanges();

            _logger.LogInformation("Supplier updated: {Id}", id);

            return NoContent();
        }

        // ==================================
        // DELETE SUPPLIER
        // ==================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _supplierRepository.DeleteAsync(id);

            if (!result)
                return NotFound();

            await _supplierRepository.SaveChanges();

            _logger.LogWarning("Supplier deleted: {Id}", id);

            return NoContent();
        }
    }
}