using AutoMapper;
using BLL.DTO;
using BLL.DTO.Adding;
using DAL.Entities;
using DAL.UnitsOfWork;

namespace BLL.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<OrderDTO>> GetAllAsync()
        {
            var ordersToMap = await unitOfWork.OrderRepository.GetAllAsync();

            return ordersToMap.Select(mapper.Map<OrderDTO>).ToList();
        }

        public async Task<OrderDTO?> GetByIdAsync(int id)
        {
            return mapper.Map<OrderDTO>(await unitOfWork.OrderRepository.GetByIdAsync(id));
        }

        public async Task<OrderDTO?> AddAsync(OrderAddingDTO orderToAdd)
        {
            if (await unitOfWork.WorkerRepository.GetByIdAsync(orderToAdd.WorkerId) is null
                || await unitOfWork.CustomerRepository.GetByIdAsync(orderToAdd.CustomerId) is null)
                return null;

            var addedOrder = await unitOfWork.OrderRepository.AddAsync(mapper.Map<Order>(orderToAdd));

            return mapper.Map<OrderDTO>(addedOrder);
        }

        public async Task<OrderDTO?> UpdateAsync(OrderDTO orderToUpdate)
        {
            if (await unitOfWork.WorkerRepository.GetByIdAsync(orderToUpdate.WorkerId) is null
                || await unitOfWork.CustomerRepository.GetByIdAsync(orderToUpdate.CustomerId) is null)
                return null;

            var updatedOrder = await unitOfWork.OrderRepository.UpdateAsync(mapper.Map<Order>(orderToUpdate));

            return mapper.Map<OrderDTO?>(updatedOrder);
        }

        public async Task<bool> DeleteAsync(int orderId)
        {
            return await unitOfWork.OrderRepository.DeleteAsync(orderId);
        }
    }
}
