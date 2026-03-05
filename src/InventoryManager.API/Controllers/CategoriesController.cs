using AutoMapper;
using InventoryManager.API.Data;
using InventoryManager.API.Data.Entities;
using InventoryManager.API.Models;
using InventoryManager.API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly InventoryManagerContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(InventoryManagerContext context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var list = _context.Categories.ToList();

            //var categoriesDto = _context.Categories.Select(c => new CategoryDto
            //{
            //    Id = c.Id,
            //    Name = c.Name,
            //    Description = c.Description
            //}).ToList();

            var response = _mapper.Map<List<CategoryDto>>(list);
            var apiResponse = ApiResponse<List<CategoryDto>>.SuccessResponse(response);

            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                //return NotFound();
                return NotFound(ApiResponse<CategoryDto>.FailureResponse("Category not found", 404));
            }
            //var result = new CategoryDto
            //{
            //    Id = category.Id,
            //    Name = category.Name,
            //    Description = category.Description
            //};

            var response = _mapper.Map<CategoryDto>(category);
            var apiResponse = ApiResponse<CategoryDto>.SuccessResponse(response);

            return Ok(apiResponse);
        }

        [HttpPost]
        public IActionResult Create(CategoryDto categoryRequest)
        {
            if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            {
                //return BadRequest("Invalid category data. Name must not be empty.");
                return BadRequest(ApiResponse<object>.FailureResponse("Invalid category data. Name must not be empty.", 400));
            }

            //var category = new Category
            //{
            //    Name = categoryRequest.Name,
            //    Description = categoryRequest.Description
            //};

            var response = _mapper.Map<Category>(categoryRequest);

            _context.Categories.Add(response);
            _context.SaveChanges();

            var apiResponse = ApiResponse<object>.SuccessResponse(new { Id = response.Id }, "Category created successfully", 201);
            //return Ok(new { Id = category.Id });
            return Ok(apiResponse);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryDto categoryRequest)
        {

            if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            {
                //return BadRequest("Invalid category data. Name must not be empty.");
                return BadRequest(ApiResponse<object>.FailureResponse("Invalid category data. Name must not be empty.", 400));
            }

            if (id != categoryRequest.Id)
            {
                //return BadRequest("Category ID mismatch.");
                return BadRequest(ApiResponse<object>.FailureResponse("Category ID mismatch.", 400));
            }

            var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (existingCategory == null)
            {
                //return NotFound();
                return NotFound(ApiResponse<object>.FailureResponse("Category not found", 404));
            }

            existingCategory.Name = categoryRequest.Name;
            existingCategory.Description = categoryRequest.Description;

            _context.Update(existingCategory);
            _context.SaveChanges();

            var apiResponse = ApiResponse<object>.SuccessResponse(null, "Category updated successfully", 200);

            //return NoContent();
            return Ok(apiResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                //return NotFound();
                return NotFound(ApiResponse<object>.FailureResponse("Category not found", 404));
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();

            var apiResponse = ApiResponse<object>.SuccessResponse(null, "Category deleted successfully", 200);
            //return NoContent();
            return Ok(apiResponse);
        }
    }
}
