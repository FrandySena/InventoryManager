using AutoMapper;
using InventoryManager.Application.Dtos;
using InventoryManager.Application.Dtos.Catalogs;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Entities.Catalogs;
namespace InventoryManager.API.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<InventoryMovement, InventoryMovementDto>().ReverseMap();
        }
    }
}