using InventoryManager.API.Data;
using InventoryManager.API.Data.Entities;
using InventoryManager.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly InventoryManagerContext _context;
        public CategoriesController(InventoryManagerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categoriesDto = _context.Categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
            return Ok(categoriesDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            var result = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(CategoryDto categoryRequest)
        {
            if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            {
                return BadRequest("Invalid category data. Name must not be empty.");
            }

            var category = new Category
            {
                Name = categoryRequest.Name,
                Description = categoryRequest.Description
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok(new { Id = category.Id });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryDto categoryRequest)
        {

            if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            {
                return BadRequest("Invalid category data. Name must not be empty.");
            }

            if (id != categoryRequest.Id)
            {
                return BadRequest("Category ID mismatch.");
            }

            var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = categoryRequest.Name;
            existingCategory.Description = categoryRequest.Description;

            _context.Update(existingCategory);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
