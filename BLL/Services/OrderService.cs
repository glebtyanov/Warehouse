using AutoMapper;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
using BLL.Exceptions;
using DAL.Entities;
using DAL.Enum;
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
            var foundOrder = mapper.Map<OrderPlainDTO>(await unitOfWork.OrderRepository.GetByIdAsync(id));

            if (foundOrder is null)
                throw new NotFoundException("Order not found");

            return foundOrder;
        }

        public async Task<OrderDetailsDTO?> GetDetailsByIdAsync(int id)
        {
            var foundOrder = mapper.Map<OrderDetailsDTO>(await unitOfWork.OrderRepository.GetDetailsAsync(id));

            if (foundOrder is null)
                throw new NotFoundException("Order not found");

            return foundOrder;
        }

        public async Task<List<OrderPlainDTO>> GetAllOfGivenWorker(int workerId)
        {
            var worker = await unitOfWork.WorkerRepository.GetByIdAsync(workerId);

            if (worker is null)
                throw new NotFoundException("Worker not found");

            var allOrders = await unitOfWork.OrderRepository.GetAllAsync();

            var workersOrders = allOrders.Where(order => order.WorkerId == worker!.WorkerId).ToList();

            return workersOrders.Select(mapper.Map<OrderPlainDTO>).ToList();
        }

        public async Task<OrderPlainDTO?> AddAsync(OrderAddingDTO orderToAdd)
        {
            if (await unitOfWork.WorkerRepository.GetByIdAsync(orderToAdd.WorkerId) is null
                || await unitOfWork.CustomerRepository.GetByIdAsync(orderToAdd.CustomerId) is null
                || await unitOfWork.StatusRepository.GetByIdAsync(orderToAdd.StatusId) is null)
                throw new NotValidException("Order is not valid. Check workerId, customerId, statusId");

            orderToAdd.StatusId = (int)Enums.Statuses.WaitingForPayment;

            var addedOrder = await unitOfWork.OrderRepository.AddAsync(mapper.Map<Order>(orderToAdd));

            return mapper.Map<OrderPlainDTO>(addedOrder);
        }

        public async void AddProductToOrderAsync(OrderProductAddingDTO orderProductToAdd)
        {
            var orderProduct = mapper.Map<OrderProduct>(orderProductToAdd);

            if (unitOfWork.OrderProductRepository.Exists(orderProduct))
                throw new AlreadyExistsException("Product is already in the order");

            if (await unitOfWork.ProductRepository.GetByIdAsync(orderProduct.ProductId) is null)
                throw new NotFoundException("Order not found");

            if (await unitOfWork.OrderRepository.GetByIdAsync(orderProduct.OrderId) is null)
                throw new NotFoundException("Order not found");

            await unitOfWork.OrderProductRepository.AddAsync(orderProduct);

            return;
        }

        public async Task<OrderPlainDTO?> UpdateAsync(OrderPlainDTO orderToUpdate)
        {
            if (await unitOfWork.WorkerRepository.GetByIdAsync(orderToUpdate.WorkerId) is null
                || await unitOfWork.CustomerRepository.GetByIdAsync(orderToUpdate.CustomerId) is null
                || await unitOfWork.StatusRepository.GetByIdAsync(orderToUpdate.StatusId) is null)
                throw new NotValidException("Order is not valid. Check workerId, customerId, statusId");

            var updatedOrder = await unitOfWork.OrderRepository.UpdateAsync(mapper.Map<Order>(orderToUpdate));

            if (updatedOrder is null)
                throw new NotFoundException("Order not found");

            return mapper.Map<OrderPlainDTO?>(updatedOrder);
        }

        public async void DeleteAsync(int orderId)
        {
            var isDeleted = await unitOfWork.OrderRepository.DeleteAsync(orderId);

            if (!isDeleted)
                throw new NotFoundException("Order not found");

            return;
        }
    }
}
