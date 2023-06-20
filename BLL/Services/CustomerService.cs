using AutoMapper;
using BLL.DTO;
using BLL.DTO.Adding;
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

        public async Task<List<CustomerDTO>> GetAllAsync()
        {
            var customersToMap = await unitOfWork.CustomerRepository.GetAllAsync();

            return customersToMap.Select(mapper.Map<CustomerDTO>).ToList();
        }

        public async Task<CustomerDTO?> GetByIdAsync(int id)
        {
            return mapper.Map<CustomerDTO>(await unitOfWork.CustomerRepository.GetByIdAsync(id));
        }

        public async Task<CustomerDTO> AddAsync(CustomerAddingDTO customerToAdd)
        {
            var addedCustomer = await unitOfWork.CustomerRepository.AddAsync(mapper.Map<Customer>(customerToAdd));

            return mapper.Map<CustomerDTO>(addedCustomer);
        }

        public async Task<CustomerDTO?> UpdateAsync(CustomerDTO customerToUpdate)
        {
            var updatedCustomer = await unitOfWork.CustomerRepository.UpdateAsync(mapper.Map<Customer>(customerToUpdate));

            return mapper.Map<CustomerDTO?>(updatedCustomer);
        }

        public async Task<bool> DeleteAsync(int customerId)
        {
            return await unitOfWork.CustomerRepository.DeleteAsync(customerId);
        }
    }
}
