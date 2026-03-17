using InventoryManager.Application.Dtos.Catalogs;
using InventoryManager.Application.Responses;
using InventoryManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CatalogService _catalogService;
        public CategoriesController(CatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public ApiResponse<List<CategoryDto>> GetCategories() => _catalogService.GetAllCategories();

        [HttpGet("{id}")]
        public ApiResponse<CategoryDto> GetCategoryById(int id) => _catalogService.GetCategoryById(id);

        [HttpPost]
        public IActionResult Create(CategoryDto categoryRequest) => Ok(_catalogService.CreateCategory(categoryRequest));

        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryDto categoryRequest) => Ok(_catalogService.UpdateCategory(id, categoryRequest));

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => Ok(_catalogService.DeleteCategory(id));
    }
}
