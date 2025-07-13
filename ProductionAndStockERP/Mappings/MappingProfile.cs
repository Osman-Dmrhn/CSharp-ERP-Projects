using AutoMapper;
using ProductionAndStockERP.Dtos.OrderDtos;
using ProductionAndStockERP.Dtos.ProductionOrder;
using ProductionAndStockERP.Dtos.UserDtos;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserDto, User>().ReverseMap();

            CreateMap<UpdateUserDto, User>().ReverseMap();

            CreateMap<OrderCreateDto, Order>().ReverseMap();

            CreateMap<OrderUpdateDto, Order>().ReverseMap();

            CreateMap<ProductionOrderCreateDto,Order>().ReverseMap();

            CreateMap<ProductionOrderUpdateDto, ProductionOrder>().ReverseMap();
        }
    }
}
