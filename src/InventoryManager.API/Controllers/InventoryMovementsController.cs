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
    public class InventoryMovementsController : ControllerBase
    {
        private readonly InventoryManagerContext _context;
        private readonly IMapper _mapper;

        public InventoryMovementsController(InventoryManagerContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetInventoryMovements()
        {
            var list = _context.InventoryMovements.ToList();
            //var inventoryMovementsDto = _context.InventoryMovements.Select(i => new InventoryMovementDto
            //{
            //    Id = i.Id,
            //    ProductId = i.ProductId,
            //    Quantity = i.Quantity,
            //    Type = i.Type,
            //    Date = i.Date.ToString("yyyy-MM-dd HH:mm:ss"),
            //    Reason = i.Reason
            //}).ToList();

            var response = _mapper.Map<List<InventoryMovementDto>>(list);
            var apiResponse = ApiResponse<List<InventoryMovementDto>>.SuccessResponse(response);

            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public IActionResult GetInventoryMovementById(int id)
        {
            var inventoryMovement = _context.InventoryMovements.FirstOrDefault(p => p.Id == id);
            if (inventoryMovement == null)
            {
                //return NotFound();
                return NotFound(ApiResponse<InventoryMovementDto>.FailureResponse("Inventory movement not found", 404));
            }
            //var result = new InventoryMovementDto
            //{
            //    Id = inventoryMovement.Id,
            //    ProductId = inventoryMovement.ProductId,
            //    Quantity = inventoryMovement.Quantity,
            //    Type = inventoryMovement.Type,
            //    Date = inventoryMovement.Date.ToString("yyyy-MM-dd HH:mm:ss"),
            //    Reason = inventoryMovement.Reason
            //};

            var response = _mapper.Map<InventoryMovementDto>(inventoryMovement);
            var apiResponse = ApiResponse<InventoryMovementDto>.SuccessResponse(response);
            return Ok(apiResponse);
        }

        [HttpPost]
        public IActionResult Create(InventoryMovementDto inventoryMovementRequest)
        {
            if (inventoryMovementRequest.ProductId <= 0 || inventoryMovementRequest.Quantity < 0)
            {
                //return BadRequest("Invalid inventoryMovement data. Id or ProductId must not be 0 or negative, and stock quantity must be non-negative.");
                return BadRequest(ApiResponse<InventoryMovementDto>.FailureResponse("Invalid inventory movement data. ProductId must be greater than 0 and quantity must be non-negative.", 400));
            }
            //var inventoryMovement = new InventoryMovement
            //{
            //    ProductId = inventoryMovementRequest.ProductId,
            //    Quantity = inventoryMovementRequest.Quantity,
            //    Type = inventoryMovementRequest.Type,
            //    Reason = inventoryMovementRequest.Reason
            //};

            var response = _mapper.Map<InventoryMovement>(inventoryMovementRequest);

            _context.InventoryMovements.Add(response);
            _context.SaveChanges();

            var apiResponse = ApiResponse<InventoryMovementDto>.SuccessResponse(_mapper.Map<InventoryMovementDto>(response), "Inventory movement created successfully", 201);
            //return Ok(inventoryMovement);
            return Ok(apiResponse);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, InventoryMovementDto inventoryMovementRequest)
        {

            if (inventoryMovementRequest.Id <= 0 || inventoryMovementRequest.ProductId <= 0 || inventoryMovementRequest.Quantity < 0)
            {
                //return BadRequest("Invalid inventoryMovement data. Id or ProductId must not be 0 or negative, and stock quantity must be non-negative.");
                return BadRequest(ApiResponse<InventoryMovementDto>.FailureResponse("Invalid inventory movement data. Id and ProductId must be greater than 0 and quantity must be non-negative.", 400));
            }

            if (id != inventoryMovementRequest.Id)
            {
                //return BadRequest("InventoryMovement ID mismatch.");
                return BadRequest(ApiResponse<InventoryMovementDto>.FailureResponse("Inventory movement ID mismatch.", 400));
            }

            var existingInventoryMovement = _context.InventoryMovements.FirstOrDefault(p => p.Id == id);
            if (existingInventoryMovement == null)
            {
                //return NotFound();
                return NotFound(ApiResponse<InventoryMovementDto>.FailureResponse("Inventory movement not found", 404));
            }

            existingInventoryMovement.ProductId = inventoryMovementRequest.ProductId;
            existingInventoryMovement.Quantity = inventoryMovementRequest.Quantity;
            existingInventoryMovement.Type = inventoryMovementRequest.Type;
            existingInventoryMovement.Reason = inventoryMovementRequest.Reason;
            existingInventoryMovement.Date = DateTime.Now;

            _context.SaveChanges();

            var apiResponse = ApiResponse<InventoryMovementDto>.SuccessResponse(_mapper.Map<InventoryMovementDto>(existingInventoryMovement), "Inventory movement updated successfully", 200);

            //return NoContent();
            return Ok(apiResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var inventoryMovement = _context.InventoryMovements.FirstOrDefault(p => p.Id == id);
            if (inventoryMovement == null)
            {
                //return NotFound();
                return NotFound(ApiResponse<InventoryMovementDto>.FailureResponse("Inventory movement not found", 404));
            }
            _context.InventoryMovements.Remove(inventoryMovement);
            _context.SaveChanges();

            var apiResponse = ApiResponse<object>.SuccessResponse(null, "Inventory movement deleted successfully", 200);
            //return NoContent();
            return Ok(apiResponse);
        }
    }
}
