using AutoMapper;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
using BLL.Exceptions;
using DAL.Entities;
using DAL.UnitsOfWork;

namespace BLL.Services
{
    public class CustomerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<CustomerPlainDTO>> GetAllAsync()
        {
            var customersToMap = await unitOfWork.CustomerRepository.GetAllAsync();

            return customersToMap.Select(mapper.Map<CustomerPlainDTO>).ToList();
        }

        public async Task<CustomerPlainDTO> GetByIdAsync(int id)
        {
            var foundCustomer = mapper.Map<CustomerPlainDTO>(await unitOfWork.CustomerRepository.GetByIdAsync(id));

            if (foundCustomer is null)
                throw new NotFoundException("Customer not found");

            return foundCustomer;
        }

        public async Task<CustomerDetailsDTO> GetDetailsByIdAsync(int id)
        {
            var foundCustomer = mapper.Map<CustomerDetailsDTO>(await unitOfWork.CustomerRepository.GetDetailsAsync(id));
        
            if (foundCustomer is null)
                throw new NotFoundException("Customer not found");

            return foundCustomer;
        }

        public async Task<CustomerPlainDTO> AddAsync(CustomerAddingDTO customerToAdd)
        {
            var addedCustomer = await unitOfWork.CustomerRepository.AddAsync(mapper.Map<Customer>(customerToAdd));

            return mapper.Map<CustomerPlainDTO>(addedCustomer);
        }

        public async Task<CustomerPlainDTO> UpdateAsync(CustomerPlainDTO customerToUpdate)
        {
            var updatedCustomer = await unitOfWork.CustomerRepository.UpdateAsync(mapper.Map<Customer>(customerToUpdate));

            if (updatedCustomer is null)
                throw new NotFoundException("Customer not found");

            return mapper.Map<CustomerPlainDTO>(updatedCustomer);
        }

        public async void DeleteAsync(int customerId)
        {
            var isDeleted = await unitOfWork.CustomerRepository.DeleteAsync(customerId);

            if(!isDeleted)
                throw new NotFoundException("Customer not found");

            return;
        }
    }
}
