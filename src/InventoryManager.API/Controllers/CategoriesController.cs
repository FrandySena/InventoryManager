using InventoryManager.API.Models;
using InventoryManager.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {

        private static readonly List<Category> _Categories = new List<Category>
        {
            new Category { Id = 1, Name = "Laptops", Description = "Portable computers" },
            new Category { Id = 2, Name = "Smartphones", Description = "Mobile phones with advanced features" }
        };

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categoriesDto = _Categories.Select(c => new CategoryDto
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
            var category = _Categories.FirstOrDefault(c => c.Id == id);
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
            int newId = _Categories.Count > 0 ? _Categories.Max(p => p.Id) + 1 : 1;
            var category = new Category
            {
                Id = newId,
                Name = categoryRequest.Name,
                Description = categoryRequest.Description
            };

            _Categories.Add(category);
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

            var existingCategory = _Categories.FirstOrDefault(c => c.Id == id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = categoryRequest.Name;
            existingCategory.Description = categoryRequest.Description;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _Categories.Remove(category);
            return NoContent();
        }
    }
}
