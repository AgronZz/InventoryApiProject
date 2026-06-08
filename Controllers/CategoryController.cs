using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventoryApiProject.Dtos;
using InventoryApiProject.Models;
using InventoryApiProject.Repositories;
using InventoryApiProject.Dtos;
using InventoryApiProject.Models;
using InventoryApi.Repositories;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  //Every endpoint requires login
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET ALL
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync();  //Get categories from repository
            var result = _mapper.Map<List<CategoryDto>>(categories);  //convert category to categoryDto using automapper

            return Ok(result);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();  //prevents invalid GUIDs from being processed

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return Ok(_mapper.Map<CategoryDto>(category));
        }

        // CREATE
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            if (dto == null)
                return BadRequest();

            var category = _mapper.Map<Category>(dto);  //Convert DTO

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChanges();

            _logger.LogInformation($"Category created: {category.Name}");

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);  //returns 201 Created with location header
        }

        // UPDATE
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateCategoryDto dto)
        {
            if (id == Guid.Empty || dto == null)
                return BadRequest();

            var category = _mapper.Map<Category>(dto); //Maps DTO → Category

            var result = await _categoryRepository.UpdateAsync(id, category); //calls the method from repo

            if (!result)
                return NotFound();  //return not found if there is no result

            await _categoryRepository.SaveChanges();  //saves changes to the database

            _logger.LogInformation($"Category updated: {id}");

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryRepository.DeleteAsync(id);

            if (!result)
                return NotFound();

            await _categoryRepository.SaveChanges();

            _logger.LogWarning($"Category deleted: {id}");

            return NoContent();
        }
    }
}