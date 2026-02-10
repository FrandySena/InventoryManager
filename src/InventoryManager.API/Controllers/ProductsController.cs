using InventoryManager.API.Data;
using InventoryManager.API.Data.Entities;
using InventoryManager.API.Models;
using InventoryManager.API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly InventoryManagerContext _context;

        public ProductsController(InventoryManagerContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var productsDto = _context.Products.Select(p => new ProductDto
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
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
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

        [HttpGet("with-categories")]
        public IActionResult GetAll()
        {
            var productsWithCategories = _context.Products.Select(p => new ProductsWithCategory()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                Category = new CategoryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name,
                    Description = p.Category.Description
                }
            }).ToList();

            return Ok(productsWithCategories);
        }

        [HttpPost]
        public IActionResult Create(ProductDto productRequest)
        {
            if (string.IsNullOrWhiteSpace(productRequest.Name) || productRequest.Price <= 0 || productRequest.StockQuantity < 0)
            {
                return BadRequest("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.");
            }
            var product = new Product
            {
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price,
                StockQuantity = productRequest.StockQuantity,
                CategoryId = productRequest.CategoryId
            };

            _context.Products.Add(product);
            _context.SaveChanges();
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

            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);
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
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            return NoContent();
        }
    }
}
