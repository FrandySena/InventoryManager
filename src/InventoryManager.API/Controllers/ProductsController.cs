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
    public class ProductsController : ControllerBase
    {
        private readonly InventoryManagerContext _context;
        private readonly IMapper _mapper;

        public ProductsController(InventoryManagerContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var list = _context.Products.ToList();
            //var productsDto = _context.Products.Select(p => new ProductDto
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Description = p.Description,
            //    Price = p.Price,
            //    StockQuantity = p.StockQuantity,
            //    CategoryId = p.CategoryId
            //}).ToList();

            var response = _mapper.Map<List<ProductDto>>(list);
            var apiResponse = ApiResponse<List<ProductDto>>.SuccessResponse(response);

            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                //return NotFound();
                return NotFound(ApiResponse<ProductDto>.FailureResponse("Product not found", 404));
            }
            //var result = new ProductDto
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    StockQuantity = product.StockQuantity,
            //    CategoryId = product.CategoryId
            //};

            var result = _mapper.Map<ProductDto>(product);
            var apiResponse = ApiResponse<ProductDto>.SuccessResponse(result);
            
            return Ok(apiResponse);
        }

        [HttpGet("with-categories")]
        public IActionResult GetAll()
        {

            var list = _context.Products.ToList();

            //var productsWithCategories = _context.Products.Select(p => new ProductsWithCategory()
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Description = p.Description,
            //    Price = p.Price,
            //    StockQuantity = p.StockQuantity,
            //    Category = new CategoryDto
            //    {
            //        Id = p.Category.Id,
            //        Name = p.Category.Name,
            //        Description = p.Category.Description
            //    }
            //}).ToList();

            var productsWithCategories = _mapper.Map<List<ProductsWithCategory>>(list);
            var apiResponse = ApiResponse<List<ProductsWithCategory>>.SuccessResponse(productsWithCategories);

            return Ok(apiResponse);
        }

        [HttpPost]
        public IActionResult Create(ProductDto productRequest)
        {
            if (string.IsNullOrWhiteSpace(productRequest.Name) || productRequest.Price <= 0 || productRequest.StockQuantity < 0)
            {
                //return BadRequest("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.");
                return BadRequest(ApiResponse<ProductDto>.FailureResponse("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.", 400));
            }
            //var product = new Product
            //{
            //    Name = productRequest.Name,
            //    Description = productRequest.Description,
            //    Price = productRequest.Price,
            //    StockQuantity = productRequest.StockQuantity,
            //    CategoryId = productRequest.CategoryId
            //};

            var product = _mapper.Map<Product>(productRequest);

            _context.Products.Add(product);
            _context.SaveChanges();

            var apiResponse = ApiResponse<ProductDto>.SuccessResponse(productRequest, "Product created successfully", 201);
            return Ok(apiResponse);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductDto productRequest)
        {

            if (string.IsNullOrWhiteSpace(productRequest.Name) || productRequest.Price <= 0 || productRequest.StockQuantity < 0)
            {
                //return BadRequest("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.");
                return BadRequest(ApiResponse<ProductDto>.FailureResponse("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.", 400));
            }

            if (id != productRequest.Id)
            {
                //return BadRequest("Product ID mismatch.");
                return BadRequest(ApiResponse<ProductDto>.FailureResponse("Product ID mismatch.", 400));
            }

            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                //return NotFound();
                return NotFound(ApiResponse<ProductDto>.FailureResponse("Product not found", 404));
            }

            existingProduct.Name = productRequest.Name;
            existingProduct.Description = productRequest.Description;
            existingProduct.Price = productRequest.Price;
            existingProduct.StockQuantity = productRequest.StockQuantity;
            existingProduct.CategoryId = productRequest.CategoryId;

            _context.SaveChanges();

            //return NoContent();
            var apiResponse = ApiResponse<ProductDto>.SuccessResponse(productRequest, "Product updated successfully", 200);
            return Ok(apiResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                //return NotFound();
                return NotFound(ApiResponse<ProductDto>.FailureResponse("Product not found", 404));
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            var apiResponse = ApiResponse<ProductDto>.SuccessResponse(null, "Product deleted successfully", 200);

            //return NoContent();
            return Ok(apiResponse);
        }
    }
}
