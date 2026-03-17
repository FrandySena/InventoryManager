using InventoryManager.Application.Dtos.Catalogs;
using InventoryManager.Application.Responses;
using InventoryManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly CatalogService _catalogService;

        public ProductsController(CatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public ApiResponse<List<ProductDto>> GetProducts() => _catalogService.GetAllProducts();

        [HttpGet("{id}")]
        public ApiResponse<ProductDto> GetById(int id) => _catalogService.GetProductById(id);


        [HttpGet("with-categories")]
        public IActionResult GetAll() => Ok(_catalogService.GetAllProductsWithCategory());

        [HttpPost]
        public IActionResult Create(ProductDto productRequest) => Ok(_catalogService.CreateProduct(productRequest));

        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductDto productRequest) => Ok(_catalogService.UpdateProduct(id, productRequest));

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => Ok(_catalogService.DeleteProduct(id));

    }
}
