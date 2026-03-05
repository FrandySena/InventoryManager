using AutoMapper;
using InventoryManager.API.Models;
using InventoryManager.API.Models.Dtos;
using InventoryManager.Domain.Entities;
using InventoryManager.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ProductRepository _productRepository;
        private readonly UnitOfWork _unitOfWork;

        public ProductsController(IMapper mapper, 
            ProductRepository productRepository, 
            UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var list = _productRepository.GetAll();

            var response = _mapper.Map<List<ProductDto>>(list);
            var apiResponse = ApiResponse<List<ProductDto>>.SuccessResponse(response);

            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound(ApiResponse<ProductDto>.FailureResponse("Product not found", 404));
            }

            var result = _mapper.Map<ProductDto>(product);
            var apiResponse = ApiResponse<ProductDto>.SuccessResponse(result);

            return Ok(apiResponse);
        }

        [HttpGet("with-categories")]
        public IActionResult GetAll()
        {

            var list = _productRepository.GetAll();

            var productsWithCategories = _productRepository.GetProductsWithCategory()
                .Select(p => new ProductsWithCategory
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

            var apiResponse = ApiResponse<List<ProductsWithCategory>>.SuccessResponse(productsWithCategories);

            return Ok(apiResponse);
        }

        [HttpPost]
        public IActionResult Create(ProductDto productRequest)
        {
            if (string.IsNullOrWhiteSpace(productRequest.Name) || productRequest.Price <= 0 || productRequest.StockQuantity < 0)
            {
                return BadRequest(ApiResponse<ProductDto>.FailureResponse("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.", 400));
            }

            var product = _mapper.Map<Product>(productRequest);

            _unitOfWork.BeginTransaction();
            _productRepository.Add(product);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            var apiResponse = ApiResponse<ProductDto>.SuccessResponse(null, "Product created successfully", 201);
            return Ok(apiResponse);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductDto productRequest)
        {

            if (string.IsNullOrWhiteSpace(productRequest.Name) || productRequest.Price <= 0 || productRequest.StockQuantity < 0)
            {
                return BadRequest(ApiResponse<ProductDto>.FailureResponse("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.", 400));
            }

            if (id != productRequest.Id)
            {
                return BadRequest(ApiResponse<ProductDto>.FailureResponse("Product ID mismatch.", 400));
            }

            var existingProduct = _productRepository.GetById(id);
            if (existingProduct == null)
            {
                return NotFound(ApiResponse<ProductDto>.FailureResponse("Product not found", 404));
            }

            _unitOfWork.BeginTransaction();
            existingProduct.Name = productRequest.Name;
            existingProduct.Description = productRequest.Description;
            existingProduct.Price = productRequest.Price;
            existingProduct.StockQuantity = productRequest.StockQuantity;
            existingProduct.CategoryId = productRequest.CategoryId;


            _productRepository.Update(existingProduct);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            var apiResponse = ApiResponse<ProductDto>.SuccessResponse(null, "Product updated successfully", 200);
            return Ok(apiResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound(ApiResponse<ProductDto>.FailureResponse("Product not found", 404));
            }

            var apiResponse = ApiResponse<ProductDto>.SuccessResponse(null, "Product deleted successfully", 200);

            _unitOfWork.BeginTransaction();
            _productRepository.Delete(id);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            return Ok(apiResponse);
        }
    }
}
