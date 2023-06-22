using AutoMapper;
using BLL.DTO.Plain;
using DAL.Entities;
using DAL.UnitsOfWork;
using BLL.Exceptions;

namespace BLL.Services
{
    public class WorkerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public WorkerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<WorkerPlainDTO>> GetAllAsync()
        {
            var workersToMap = await unitOfWork.WorkerRepository.GetAllAsync();

            return workersToMap.Select(mapper.Map<WorkerPlainDTO>).ToList();
        }

        public async Task<WorkerPlainDTO> GetByIdAsync(int id)
        {
            var foundWorker = mapper.Map<WorkerPlainDTO>(await unitOfWork.WorkerRepository.GetByIdAsync(id));

            if (foundWorker is null)
                throw new NotFoundException("Worker not found");

            return foundWorker;
        }

        public async Task<WorkerDetailsDTO> GetDetailsByIdAsync(int id)
        {
            var foundWorker = mapper.Map<WorkerDetailsDTO>(await unitOfWork.WorkerRepository.GetDetailsAsync(id));

            if (foundWorker is null)
                throw new NotFoundException("Worker not found");

            return foundWorker;
        }

        public async Task<WorkerPlainDTO> UpdateAsync(WorkerPlainDTO workerToUpdate)
        {
            if (await unitOfWork.PositionRepository.GetByIdAsync(workerToUpdate.PositionId) is null)
                throw new NotValidException("Invalid position");

            var updatedWorker = await unitOfWork.WorkerRepository.UpdateAsync(mapper.Map<Worker>(workerToUpdate));

            if (updatedWorker is null)
                throw new NotFoundException("Worker not found");

            return mapper.Map<WorkerPlainDTO>(updatedWorker);
        }

        public async void DeleteAsync(int workerId)
        {
            var isDeleted = await unitOfWork.WorkerRepository.DeleteAsync(workerId);

            if (!isDeleted)
                throw new NotValidException("Worker not found");

            return;
        }

    }
}
