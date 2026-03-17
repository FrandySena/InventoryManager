using AutoMapper;
using InventoryManager.Application.Dtos.Catalogs;
using InventoryManager.Application.Responses;
using InventoryManager.Domain.Entities.Catalogs;
using InventoryManager.Infrastructure.Repositories;

namespace InventoryManager.Application.Services
{
    public class CatalogService
    {


        private readonly IMapper _mapper;
        private readonly ProductRepository _productRepository;
        private readonly GenericRepository<Category> _categoryRepository;
        private readonly UnitOfWork _unitOfWork;

        public CatalogService(IMapper mapper,
            ProductRepository productRepository,
            UnitOfWork unitOfWork,
            GenericRepository<Category> categoryRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public ApiResponse<List<ProductDto>> GetAllProducts()
        {
            var list = _productRepository.GetAll();

            var response = _mapper.Map<List<ProductDto>>(list);
            var apiResponse = ApiResponse<List<ProductDto>>.SuccessResponse(response);

            return apiResponse;
        }

        public ApiResponse<ProductDto> GetProductById(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return ApiResponse<ProductDto>.FailureResponse("Product not found", 404);
            }

            var result = _mapper.Map<ProductDto>(product);
            var apiResponse = ApiResponse<ProductDto>.SuccessResponse(result);

            return apiResponse;
        }

        public ApiResponse<List<ProductsWithCategory>> GetAllProductsWithCategory()
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

            return apiResponse;
        }

        public ApiResponse<ProductDto> CreateProduct(ProductDto productRequest)
        {
            if (string.IsNullOrWhiteSpace(productRequest.Name) || productRequest.Price <= 0 || productRequest.StockQuantity < 0)
            {
                return ApiResponse<ProductDto>.FailureResponse("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.", 400);
            }

            var product = _mapper.Map<Product>(productRequest);

            _unitOfWork.BeginTransaction();
            _productRepository.Add(product);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            var apiResponse = ApiResponse<ProductDto>.SuccessResponse(null, "Product created successfully", 201);
            return apiResponse;
        }
        public ApiResponse<ProductDto> UpdateProduct(int id, ProductDto productRequest)
        {

            if (string.IsNullOrWhiteSpace(productRequest.Name) || productRequest.Price <= 0 || productRequest.StockQuantity < 0)
            {
                return ApiResponse<ProductDto>.FailureResponse("Invalid product data. Name must not be empty, price must be greater than 0, and stock quantity must be non-negative.", 400);
            }

            if (id != productRequest.Id)
            {
                return ApiResponse<ProductDto>.FailureResponse("Product ID mismatch.", 400);
            }

            var existingProduct = _productRepository.GetById(id);
            if (existingProduct == null)
            {
                return ApiResponse<ProductDto>.FailureResponse("Product not found", 404);
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
            return apiResponse;
        }

        public ApiResponse<ProductDto> DeleteProduct(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return ApiResponse<ProductDto>.FailureResponse("Product not found", 404);
            }

            var apiResponse = ApiResponse<ProductDto>.SuccessResponse(null, "Product deleted successfully", 200);

            _unitOfWork.BeginTransaction();
            _productRepository.Delete(id);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            return apiResponse;
        }

        public ApiResponse<List<CategoryDto>> GetAllCategories()
        {
            var list = _categoryRepository.GetAll();

            var response = _mapper.Map<List<CategoryDto>>(list);
            var apiResponse = ApiResponse<List<CategoryDto>>.SuccessResponse(response);

            return apiResponse;
        }

        public ApiResponse<CategoryDto> GetCategoryById(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return ApiResponse<CategoryDto>.FailureResponse("Category not found", 404);
            }

            var response = _mapper.Map<CategoryDto>(category);
            var apiResponse = ApiResponse<CategoryDto>.SuccessResponse(response);

            return apiResponse;
        }

        public ApiResponse<CategoryDto> CreateCategory(CategoryDto categoryRequest)
        {
            if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            {
                return ApiResponse<CategoryDto>.FailureResponse("Invalid category data. Name must not be empty.", 400);
            }

            var response = _mapper.Map<Category>(categoryRequest);

            _unitOfWork.BeginTransaction();
            _categoryRepository.Add(response);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            return ApiResponse<CategoryDto>.SuccessResponse(new CategoryDto { Id = response.Id }, "Category created successfully", 201);
        }

        public ApiResponse<CategoryDto> UpdateCategory(int id, CategoryDto categoryRequest)
        {

            if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            {
                return ApiResponse<CategoryDto>.FailureResponse("Invalid category data. Name must not be empty.", 400);
            }

            if (id != categoryRequest.Id)
            {
                return ApiResponse<CategoryDto>.FailureResponse("Category ID mismatch.", 400);
            }

            var existingCategory = _categoryRepository.GetById(id);
            if (existingCategory == null)
            {
                return ApiResponse<CategoryDto>.FailureResponse("Category not found", 404);
            }

            _unitOfWork.BeginTransaction();
            existingCategory.Name = categoryRequest.Name;
            existingCategory.Description = categoryRequest.Description;

            _categoryRepository.Update(existingCategory);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            var apiResponse = ApiResponse<CategoryDto>.SuccessResponse(null, "Category updated successfully", 200);

            return apiResponse;
        }

        public ApiResponse<CategoryDto> DeleteCategory(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return ApiResponse<CategoryDto>.FailureResponse("Category not found", 404);
            }

            _unitOfWork.BeginTransaction();
            _categoryRepository.Delete(id);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            var apiResponse = ApiResponse<CategoryDto>.SuccessResponse(null, "Category deleted successfully", 200);
            return apiResponse;
        }

    }
}
