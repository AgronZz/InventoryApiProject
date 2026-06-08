using AutoMapper;
using InventoryApiProject.Models;
using InventoryApiProject.Dtos;

namespace InventoryApiProject.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ProductCount,
                    opt => opt.MapFrom(src => src.Products.Count));

            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}