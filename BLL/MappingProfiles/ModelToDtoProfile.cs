using AutoMapper;
using BLL.DTO;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
using DAL.Entities;

namespace BLL.MappingProfiles
{
    internal class ModelToDtoProfile : Profile
    {
        public ModelToDtoProfile()
        {
            CreateMap<CustomerAddingDTO, Customer>().ReverseMap();
            CreateMap<CustomerPlainDTO, Customer>().ReverseMap();

            CreateMap<DepartmentAddingDTO, Department>().ReverseMap();
            CreateMap<DepartmentPlainDTO, Department>().ReverseMap();

            CreateMap<OrderAddingDTO, Order>().ReverseMap();
            CreateMap<OrderDTO, Order>().ReverseMap();
        }
    }
}
