using AutoMapper;
using InventoryApiProject.Models;
using InventoryApiProject.Dtos;

namespace InventoryApiProject.Mappings
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<Supplier, SupplierDto>()
                .ForMember(dest => dest.ProductCount,
                    opt => opt.MapFrom(src => src.Products.Count));

            CreateMap<CreateSupplierDto, Supplier>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<UpdateSupplierDto, Supplier>();
        }
    }
}