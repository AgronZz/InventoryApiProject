using AutoMapper;
using InventoryApiProject.Models;
using InventoryApiProject.Dtos;

namespace InventoryApiProject.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.Items));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"));

            CreateMap<CreateOrderItemDto, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}