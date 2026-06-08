using AutoMapper;
using InventoryApiProject.Models;
using InventoryApiProject.Dtos;

namespace InventoryApiProject.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Model ? DTO
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.SupplierName,
                    opt => opt.MapFrom(src => src.Supplier.Name));

            // Create DTO ? Model
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Update DTO ? Model
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}