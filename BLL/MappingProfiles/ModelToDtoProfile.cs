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
            CreateMap<CustomerDetailsDTO, Customer>().ReverseMap();

            CreateMap<DepartmentAddingDTO, Department>().ReverseMap();
            CreateMap<DepartmentPlainDTO, Department>().ReverseMap();
            CreateMap<DepartmentDetailsDTO, Department>().ReverseMap();

            CreateMap<OrderAddingDTO, Order>().ReverseMap();
            CreateMap<OrderPlainDTO, Order>().ReverseMap();
            CreateMap<OrderDetailsDTO, Order>().ReverseMap();

            CreateMap<PositionAddingDTO, Position>().ReverseMap();
            CreateMap<PositionPlainDTO, Position>().ReverseMap();
            CreateMap<PositionDetailsDTO, Position>().ReverseMap();

            CreateMap<ProductAddingDTO, Product>().ReverseMap();
            CreateMap<ProductPlainDTO, Product>().ReverseMap();
            CreateMap<ProductDetailsDTO, Product>().ReverseMap();

            CreateMap<StatusAddingDTO, Status>().ReverseMap();
            CreateMap<StatusPlainDTO, Status>().ReverseMap();
            CreateMap<StatusDetailsDTO, Status>().ReverseMap();

            CreateMap<TransactionAddingDTO, Transaction>().ReverseMap();
            CreateMap<TransactionPlainDTO, Transaction>().ReverseMap();
            CreateMap<TransactionDetailsDTO, Transaction>().ReverseMap();

            CreateMap<WorkerAddingDTO, Worker>().ReverseMap();
            CreateMap<WorkerPlainDTO, Worker>().ReverseMap();
            CreateMap<WorkerDetailsDTO, Worker>().ReverseMap();
            CreateMap<WorkerLoginDTO, Worker>().ReverseMap();

            CreateMap<DepartmentWorkerAddingDTO, DepartmentWorker>().ReverseMap();

            CreateMap<OrderProductAddingDTO, OrderProduct>().ReverseMap();
        }
    }
}
