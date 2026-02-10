using InventoryManager.API.Models;
using InventoryManager.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private static readonly List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop A", Description = "Is a laptop A", Price = 999.0m, StockQuantity = 100, CategoryId = 1 },
            new Product { Id = 2, Name = "PC B", Description = "Is a PC B", Price = 1299.0m, StockQuantity = 50, CategoryId = 1 },
            new Product { Id = 3, Name = "Smartphone C", Description = "Is a smartphone C", Price = 699.0m, StockQuantity = 200, CategoryId = 2 }
        };

        [HttpGet]
        public IActionResult GetProducts()
        {
            var productsDto = _products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId
            }).ToList();
            return Ok(productsDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var result = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId
            };
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(ProductDto productRequest)
        {
            if (string.IsNullOrWhiteSpace(productRequest.Name) || productRequest.Price <= 0 || productRequest.StockQuantity < 0)
            {
                return BadRequest("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.");
            }
            int newId = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            var product = new Product
            {
                Id = newId,
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price,
                StockQuantity = productRequest.StockQuantity,
                CategoryId = productRequest.CategoryId
            };

            _products.Add(product);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductDto productRequest)
        {

            if (string.IsNullOrWhiteSpace(productRequest.Name) || productRequest.Price <= 0 || productRequest.StockQuantity < 0)
            {
                return BadRequest("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.");
            }

            if (id != productRequest.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = productRequest.Name;
            existingProduct.Description = productRequest.Description;
            existingProduct.Price = productRequest.Price;
            existingProduct.StockQuantity = productRequest.StockQuantity;
            existingProduct.CategoryId = productRequest.CategoryId;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _products.Remove(product);
            return NoContent();
        }
    }
}
