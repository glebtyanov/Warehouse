using AutoMapper;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
using DAL.Entities;
using DAL.UnitsOfWork;
using BLL.Exceptions;

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

        public async Task<StatusPlainDTO> GetByIdAsync(int id)
        {
            var foundProduct = mapper.Map<StatusPlainDTO>(await unitOfWork.StatusRepository.GetByIdAsync(id));

            if (foundProduct is null)
                throw new NotFoundException("Status not found");

            return foundProduct;
        }

        public async Task<StatusDetailsDTO> GetDetailsByIdAsync(int id)
        {
            var foundProduct = mapper.Map<StatusDetailsDTO>(await unitOfWork.StatusRepository.GetDetailsAsync(id));

            if (foundProduct is null)
                throw new NotFoundException("Status not found");

            return foundProduct;
        }

        public async Task<StatusPlainDTO> AddAsync(StatusAddingDTO statusToAdd)
        {
            var addedStatus = await unitOfWork.StatusRepository.AddAsync(mapper.Map<Status>(statusToAdd));

            return mapper.Map<StatusPlainDTO>(addedStatus);
        }

        public async Task<StatusPlainDTO> UpdateAsync(StatusPlainDTO statusToUpdate)
        {
            var updatedStatus = await unitOfWork.StatusRepository.UpdateAsync(mapper.Map<Status>(statusToUpdate));

            if (updatedStatus is null)
                throw new NotFoundException("Status not found");

            return mapper.Map<StatusPlainDTO>(updatedStatus);
        }

        public async void DeleteAsync(int statusId)
        {
            var isDeleted = await unitOfWork.StatusRepository.DeleteAsync(statusId);

            if(!isDeleted)
                throw new NotFoundException("Status not found");

            return;
        }
    }
}
