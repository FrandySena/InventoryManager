using AutoMapper;
using InventoryManager.API.Models.Dtos;
using InventoryManager.Domain.Entities;
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
