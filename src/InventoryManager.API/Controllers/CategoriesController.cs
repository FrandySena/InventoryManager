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
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly GenericRepository<Category> _categoryRepository;
        private readonly UnitOfWork _unitOfWork;

        public CategoriesController(GenericRepository<Category> categoryRepository, IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var list = _categoryRepository.GetAll();

            var response = _mapper.Map<List<CategoryDto>>(list);
            var apiResponse = ApiResponse<List<CategoryDto>>.SuccessResponse(response);

            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound(ApiResponse<CategoryDto>.FailureResponse("Category not found", 404));
            }

            var response = _mapper.Map<CategoryDto>(category);
            var apiResponse = ApiResponse<CategoryDto>.SuccessResponse(response);

            return Ok(apiResponse);
        }

        [HttpPost]
        public IActionResult Create(CategoryDto categoryRequest)
        {
            if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            {
                return BadRequest(ApiResponse<object>.FailureResponse("Invalid category data. Name must not be empty.", 400));
            }

            var response = _mapper.Map<Category>(categoryRequest);

            _unitOfWork.BeginTransaction();
            _categoryRepository.Add(response);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            var apiResponse = ApiResponse<object>.SuccessResponse(new { Id = response.Id }, "Category created successfully", 201);
            return Ok(apiResponse);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryDto categoryRequest)
        {

            if (string.IsNullOrWhiteSpace(categoryRequest.Name))
            {
                return BadRequest(ApiResponse<object>.FailureResponse("Invalid category data. Name must not be empty.", 400));
            }

            if (id != categoryRequest.Id)
            {
                return BadRequest(ApiResponse<object>.FailureResponse("Category ID mismatch.", 400));
            }

            var existingCategory = _categoryRepository.GetById(id);
            if (existingCategory == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse("Category not found", 404));
            }

            _unitOfWork.BeginTransaction();
            existingCategory.Name = categoryRequest.Name;
            existingCategory.Description = categoryRequest.Description;

            _categoryRepository.Update(existingCategory);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            var apiResponse = ApiResponse<object>.SuccessResponse(null, "Category updated successfully", 200);

            return Ok(apiResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse("Category not found", 404));
            }

            _unitOfWork.BeginTransaction();
            _categoryRepository.Delete(id);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            var apiResponse = ApiResponse<object>.SuccessResponse(null, "Category deleted successfully", 200);
            return Ok(apiResponse);
        }
    }
}
