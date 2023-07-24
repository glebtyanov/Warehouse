using AutoMapper;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
using DAL.Entities;
using DAL.Enum;
using DAL.UnitsOfWork;
using BLL.Exceptions;

namespace BLL.Services
{
    public class TransactionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<TransactionPlainDTO>> GetAllAsync()
        {
            var transactionsToMap = await unitOfWork.TransactionRepository.GetAllAsync();

            return transactionsToMap.Select(mapper.Map<TransactionPlainDTO>).ToList();
        }

        public async Task<TransactionPlainDTO> GetByIdAsync(int id)
        {
            var foundTransaction = mapper.Map<TransactionPlainDTO>(await unitOfWork.TransactionRepository.GetByIdAsync(id));

            if (foundTransaction is null)
                throw new NotFoundException("Transaction not found");

            return foundTransaction;
        }

        public async Task<TransactionDetailsDTO> GetDetailsByIdAsync(int id)
        {
            var foundTransaction = mapper.Map<TransactionDetailsDTO>(await unitOfWork.TransactionRepository.GetDetailsAsync(id));

            if (foundTransaction is null)
                throw new NotFoundException("Transaction not found");

            return foundTransaction;
        }

        public async Task<TransactionPlainDTO> AddAsync(TransactionAddingDTO transactionToAdd)
        {
            var transactionOrder = await unitOfWork.OrderRepository.GetByIdAsync(transactionToAdd.OrderId);

            if (transactionOrder is null)
                throw new NotValidException("Invalid order");

            if (transactionOrder.StatusId == (int)Enums.Statuses.Processed)
                throw new NotValidException("Order is already processed");

            var addedTransaction = await unitOfWork.TransactionRepository.AddAsync(mapper.Map<Transaction>(transactionToAdd));

            // if transaction is added it means order has been paid so order status should change
            transactionOrder.StatusId = (int)Enums.Statuses.Processed;
            await unitOfWork.OrderRepository.UpdateAsync(transactionOrder);

            return mapper.Map<TransactionPlainDTO>(addedTransaction);
        }

        public async Task<TransactionPlainDTO> UpdateAsync(TransactionPlainDTO transactionToUpdate)
        {
            if (await unitOfWork.OrderRepository.GetByIdAsync(transactionToUpdate.OrderId) is null)
                throw new NotValidException("Invalid order");

            var updatedTransaction = await unitOfWork.TransactionRepository.UpdateAsync(mapper.Map<Transaction>(transactionToUpdate));

            if (updatedTransaction is null)
                throw new NotFoundException("Transaction not found");

            return mapper.Map<TransactionPlainDTO>(updatedTransaction);
        }

        public async void DeleteAsync(int transactionId)
        {
            var isDeleted = await unitOfWork.TransactionRepository.DeleteAsync(transactionId);

            if(!isDeleted)
                throw new NotFoundException("Transaction not found");

            return;
        }
    }
}
