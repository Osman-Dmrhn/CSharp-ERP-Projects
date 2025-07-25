using AutoMapper;
using ProductionAndStockERP.Dtos.OrderDtos;
using ProductionAndStockERP.Dtos.ProductDtos;
using ProductionAndStockERP.Dtos.ProductionOrder;
using ProductionAndStockERP.Dtos.ProductionOrderDtos;
using ProductionAndStockERP.Dtos.StockTransactionDtos;
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

            CreateMap<UserDto,User>().ReverseMap();

            CreateMap<OrderCreateDto, Order>().ReverseMap();

            CreateMap<OrderUpdateDto, Order>().ReverseMap();

            CreateMap<OrderDto, Order>().ReverseMap();



            CreateMap<ProductionOrderCreateDto,ProductionOrder>().ReverseMap();

            CreateMap<ProductionOrderUpdateDto, ProductionOrder>().ReverseMap();

            CreateMap<ProductionOrderDto, ProductionOrder>().ReverseMap();



            CreateMap<StockTransactionCreate, StockTransaction>().ReverseMap();

            CreateMap<ProductCreateDto,Product>().ReverseMap();
            CreateMap<ProductUpdateDto,Product>().ReverseMap();
            CreateMap<ProductDto,Product>().ReverseMap();
        }
    }
}
