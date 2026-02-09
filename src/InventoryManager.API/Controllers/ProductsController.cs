using InventoryManager.API.Models;
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

        //me quede en la 1:31:03 clase 27

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0 || product.StockQuantity < 0)
            {
                return BadRequest("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.");
            }
            int newId = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            product.Id = newId;

            _products.Add(product);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Product product)
        {

            if (string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0 || product.StockQuantity < 0)
            {
                return BadRequest("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.");
            }

            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CategoryId = product.CategoryId;

            return Ok(product);
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
            return Ok(_products);
        }
    }
}
