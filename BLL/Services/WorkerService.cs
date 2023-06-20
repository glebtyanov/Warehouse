using AutoMapper;
using DAL.Entities;
using DAL.UnitsOfWork;
using BLL.DTO.Plain;

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

        public async Task<WorkerPlainDTO?> GetByIdAsync(int id)
        {
            return mapper.Map<WorkerPlainDTO>(await unitOfWork.WorkerRepository.GetByIdAsync(id));
        }

        public async Task<WorkerPlainDTO?> UpdateAsync(WorkerPlainDTO workerToUpdate)
        {
            if (await unitOfWork.PositionRepository.GetByIdAsync(workerToUpdate.PositionId) is null)
                return null;

            var updatedWorker = await unitOfWork.WorkerRepository.UpdateAsync(mapper.Map<Worker>(workerToUpdate));

            return mapper.Map<WorkerPlainDTO?>(updatedWorker);
        }

        public async Task<bool> DeleteAsync(int workerId)
        {
            return await unitOfWork.WorkerRepository.DeleteAsync(workerId);
        }

    }
}
