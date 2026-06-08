using AutoMapper;
using InventoryApiProject.Models;
using InventoryApiProject.Dtos;

namespace InventoryApiProject.Mappings
{
    public class StockTransactionProfile : Profile
    {
        public StockTransactionProfile()
        {
            CreateMap<StockTransaction, StockTransactionDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}