using AutoMapper;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
using DAL.Entities;
using DAL.UnitsOfWork;
using Microsoft.Identity.Client;

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

        public async Task<CustomerPlainDTO?> GetByIdAsync(int id)
        {
            return mapper.Map<CustomerPlainDTO>(await unitOfWork.CustomerRepository.GetByIdAsync(id));
        }

        public async Task<CustomerDetailsDTO?> GetDetailsByIdAsync(int id)
        {
            return mapper.Map<CustomerDetailsDTO>(await unitOfWork.CustomerRepository.GetDetailsAsync(id));
        }

        public async Task<CustomerPlainDTO> AddAsync(CustomerAddingDTO customerToAdd)
        {
            var addedCustomer = await unitOfWork.CustomerRepository.AddAsync(mapper.Map<Customer>(customerToAdd));

            return mapper.Map<CustomerPlainDTO>(addedCustomer);
        }

        public async Task<CustomerPlainDTO?> UpdateAsync(CustomerPlainDTO customerToUpdate)
        {
            var updatedCustomer = await unitOfWork.CustomerRepository.UpdateAsync(mapper.Map<Customer>(customerToUpdate));

            return mapper.Map<CustomerPlainDTO?>(updatedCustomer);
        }

        public async Task<bool> DeleteAsync(int customerId)
        {
            return await unitOfWork.CustomerRepository.DeleteAsync(customerId);
        }
    }
}
