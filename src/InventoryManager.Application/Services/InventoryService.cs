using AutoMapper;
using InventoryManager.Application.Dtos;
using InventoryManager.Application.Responses;
using InventoryManager.Domain.Entities;
using InventoryManager.Infrastructure.Repositories;

namespace InventoryManager.Application.Services
{
    public class InventoryService
    {
        private readonly IMapper _mapper;
        private readonly GenericRepository<InventoryMovement> _inventoryMovementRepository;
        private readonly UnitOfWork _unitOfWork;

        public InventoryService(IMapper mapper,
            GenericRepository<InventoryMovement> inventoryMovementRepository,
            UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _inventoryMovementRepository = inventoryMovementRepository;
            _unitOfWork = unitOfWork;
        }

        public ApiResponse<List<InventoryMovementDto>> GetAllInventoryMovements()
        {
            var list = _inventoryMovementRepository.GetAll();

            var response = _mapper.Map<List<InventoryMovementDto>>(list);
            var apiResponse = ApiResponse<List<InventoryMovementDto>>.SuccessResponse(response);

            return apiResponse;
        }

        public ApiResponse<InventoryMovementDto> GetInventoryMovementById(int id)
        {
            var inventoryMovement = _inventoryMovementRepository.GetById(id);
            if (inventoryMovement == null)
            {
                return ApiResponse<InventoryMovementDto>.FailureResponse("Inventory movement not found", 404);
            }

            var response = _mapper.Map<InventoryMovementDto>(inventoryMovement);
            var apiResponse = ApiResponse<InventoryMovementDto>.SuccessResponse(response);
            return apiResponse;
        }

        public ApiResponse<InventoryMovementDto> Create(InventoryMovementDto inventoryMovementRequest)
        {
            if (inventoryMovementRequest.ProductId <= 0 || inventoryMovementRequest.Quantity < 0)
            {
                return ApiResponse<InventoryMovementDto>.FailureResponse("Invalid inventory movement data. ProductId must be greater than 0 and quantity must be non-negative.", 400);
            }

            var response = _mapper.Map<InventoryMovement>(inventoryMovementRequest);

            _unitOfWork.BeginTransaction();
            _inventoryMovementRepository.Add(response);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            var apiResponse = ApiResponse<InventoryMovementDto>.SuccessResponse(null, "Inventory movement created successfully", 201);
            return apiResponse;
        }

        public ApiResponse<InventoryMovementDto> Update(int id, InventoryMovementDto inventoryMovementRequest)
        {

            if (inventoryMovementRequest.Id <= 0 || inventoryMovementRequest.ProductId <= 0 || inventoryMovementRequest.Quantity < 0)
            {
                return ApiResponse<InventoryMovementDto>.FailureResponse("Invalid inventory movement data. Id and ProductId must be greater than 0 and quantity must be non-negative.", 400);
            }

            if (id != inventoryMovementRequest.Id)
            {
                return ApiResponse<InventoryMovementDto>.FailureResponse("Inventory movement ID mismatch.", 400);
            }

            var existingInventoryMovement = _inventoryMovementRepository.GetById(id);
            if (existingInventoryMovement == null)
            {
                return ApiResponse<InventoryMovementDto>.FailureResponse("Inventory movement not found", 404);
            }

            _unitOfWork.BeginTransaction();
            existingInventoryMovement.ProductId = inventoryMovementRequest.ProductId;
            existingInventoryMovement.Quantity = inventoryMovementRequest.Quantity;
            existingInventoryMovement.Type = inventoryMovementRequest.Type;
            existingInventoryMovement.Reason = inventoryMovementRequest.Reason;
            existingInventoryMovement.Date = DateTime.Now;

            _inventoryMovementRepository.Update(existingInventoryMovement);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            var apiResponse = ApiResponse<InventoryMovementDto>.SuccessResponse(null, "Inventory movement updated successfully", 200);

            return apiResponse;
        }

        public ApiResponse<InventoryMovementDto> Delete(int id)
        {
            var inventoryMovement = _inventoryMovementRepository.GetById(id);
            if (inventoryMovement == null)
            {
                return ApiResponse<InventoryMovementDto>.FailureResponse("Inventory movement not found", 404);
            }

            _unitOfWork.BeginTransaction();
            _inventoryMovementRepository.Delete(id);
            _unitOfWork.Complete();
            _unitOfWork.CommitTransaction();

            var apiResponse = ApiResponse<InventoryMovementDto>.SuccessResponse(null, "Inventory movement deleted successfully", 200);
            return apiResponse;
        }
    }
}
