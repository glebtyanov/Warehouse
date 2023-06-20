using AutoMapper;
using BLL.DTO;
using BLL.DTO.Adding;
using DAL.Entities;

namespace BLL.MappingProfiles
{
    internal class ModelToDtoProfile : Profile
    {
        public ModelToDtoProfile()
        {
            CreateMap<CustomerAddingDTO, Customer>().ReverseMap();
            CreateMap<CustomerDTO, Customer>().ReverseMap();

            CreateMap<DepartmentAddingDTO, Department>().ReverseMap();
            CreateMap<DepartmentDTO, Department>().ReverseMap();

            CreateMap<OrderAddingDTO, Order>().ReverseMap();
            CreateMap<OrderDTO, Order>().ReverseMap();
        }
    }
}
