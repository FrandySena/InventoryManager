using AutoMapper;
using InventoryManager.Application.Dtos;
using InventoryManager.Application.Responses;
using InventoryManager.Application.Services;
using InventoryManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryMovementsController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        public InventoryMovementsController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public ApiResponse<List<InventoryMovementDto>> GetInventoryMovements() => _inventoryService.GetAllInventoryMovements();

        [HttpGet("{id}")]
        public ApiResponse<InventoryMovementDto> GetInventoryMovementById(int id) => _inventoryService.GetInventoryMovementById(id);

        [HttpPost]
        public IActionResult Create(InventoryMovementDto inventoryMovementRequest) => Ok(_inventoryService.Create(inventoryMovementRequest));

        [HttpPut("{id}")]
        public IActionResult Update(int id, InventoryMovementDto inventoryMovementRequest) => Ok(_inventoryService.Update(id, inventoryMovementRequest));

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => Ok(_inventoryService.Delete(id));
    }
}
