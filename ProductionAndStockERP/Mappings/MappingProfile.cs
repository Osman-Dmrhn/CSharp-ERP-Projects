using AutoMapper;
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
        }
    }
}
