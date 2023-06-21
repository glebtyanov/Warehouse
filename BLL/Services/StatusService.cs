using AutoMapper;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
using DAL.Entities;
using DAL.UnitsOfWork;

namespace BLL.Services
{
    public class StatusService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public StatusService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<StatusPlainDTO>> GetAllAsync()
        {
            var statusesToMap = await unitOfWork.StatusRepository.GetAllAsync();

            return statusesToMap.Select(mapper.Map<StatusPlainDTO>).ToList();
        }

        public async Task<StatusPlainDTO?> GetByIdAsync(int id)
        {
            return mapper.Map<StatusPlainDTO>(await unitOfWork.StatusRepository.GetByIdAsync(id));
        }

        public async Task<StatusDetailsDTO?> GetDetailsByIdAsync(int id)
        {
            return mapper.Map<StatusDetailsDTO>(await unitOfWork.StatusRepository.GetDetailsAsync(id));
        }

        public async Task<StatusPlainDTO> AddAsync(StatusAddingDTO statusToAdd)
        {
            var addedStatus = await unitOfWork.StatusRepository.AddAsync(mapper.Map<Status>(statusToAdd));

            return mapper.Map<StatusPlainDTO>(addedStatus);
        }

        public async Task<StatusPlainDTO?> UpdateAsync(StatusPlainDTO statusToUpdate)
        {
            var updatedStatus = await unitOfWork.StatusRepository.UpdateAsync(mapper.Map<Status>(statusToUpdate));

            return mapper.Map<StatusPlainDTO?>(updatedStatus);
        }

        public async Task<bool> DeleteAsync(int statusId)
        {
            return await unitOfWork.StatusRepository.DeleteAsync(statusId);
        }
    }
}
