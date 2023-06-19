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
        }
    }
}
