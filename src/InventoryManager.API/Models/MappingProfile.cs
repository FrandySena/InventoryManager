using AutoMapper;
using InventoryManager.API.Data.Entities;
using InventoryManager.API.Models.Dtos;
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
