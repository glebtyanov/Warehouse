using AutoMapper;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
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

        public async Task<List<OrderPlainDTO>> GetAllAsync()
        {
            var ordersToMap = await unitOfWork.OrderRepository.GetAllAsync();

            return ordersToMap.Select(mapper.Map<OrderPlainDTO>).ToList();
        }

        public async Task<OrderPlainDTO?> GetByIdAsync(int id)
        {
            return mapper.Map<OrderPlainDTO>(await unitOfWork.OrderRepository.GetByIdAsync(id));
        }

        public async Task<OrderPlainDTO?> AddAsync(OrderAddingDTO orderToAdd)
        {
            if (await unitOfWork.WorkerRepository.GetByIdAsync(orderToAdd.WorkerId) is null
                || await unitOfWork.CustomerRepository.GetByIdAsync(orderToAdd.CustomerId) is null)
                return null;

            var addedOrder = await unitOfWork.OrderRepository.AddAsync(mapper.Map<Order>(orderToAdd));

            return mapper.Map<OrderPlainDTO>(addedOrder);
        }

        public async Task<OrderPlainDTO?> UpdateAsync(OrderPlainDTO orderToUpdate)
        {
            if (await unitOfWork.WorkerRepository.GetByIdAsync(orderToUpdate.WorkerId) is null)
                //|| await unitOfWork.CustomerRepository.GetByIdAsync(orderToUpdate.) is null)
                return null;

            var updatedOrder = await unitOfWork.OrderRepository.UpdateAsync(mapper.Map<Order>(orderToUpdate));

            return mapper.Map<OrderPlainDTO?>(updatedOrder);
        }

        public async Task<bool> DeleteAsync(int orderId)
        {
            return await unitOfWork.OrderRepository.DeleteAsync(orderId);
        }

        public async Task<List<OrderPlainDTO>> GetAllOfGivenWorker(int workerId)
        {
            // no need to check worker to not be null since invalid id will be forbidden at controller 
            var worker = await unitOfWork.WorkerRepository.GetByIdAsync(workerId);

            var allOrders = await unitOfWork.OrderRepository.GetAllAsync();

            var workersOrders = allOrders.Where(order => order.WorkerId == worker!.WorkerId).ToList();

            return workersOrders.Select(mapper.Map<OrderPlainDTO>).ToList();
        }
    }
}
