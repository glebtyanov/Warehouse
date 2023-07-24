using AutoMapper;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
using DAL.Entities;
using DAL.UnitsOfWork;
using BLL.Exceptions;

namespace BLL.Services
{
    public class PositionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PositionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<PositionPlainDTO>> GetAllAsync()
        {
            var positionsToMap = await unitOfWork.PositionRepository.GetAllAsync();

            return positionsToMap.Select(mapper.Map<PositionPlainDTO>).ToList();
        }

        public async Task<PositionPlainDTO> GetByIdAsync(int id)
        {
            var foundPosition = mapper.Map<PositionPlainDTO>(await unitOfWork.PositionRepository.GetByIdAsync(id));

            if (foundPosition is null)
                throw new NotFoundException("Position not found");

            return foundPosition;
        }

        public async Task<PositionDetailsDTO> GetDetailsByIdAsync(int id)
        {
            var foundPosition = mapper.Map<PositionDetailsDTO>(await unitOfWork.PositionRepository.GetDetailsAsync(id));

            if (foundPosition is null)
                throw new NotFoundException("Position not found");

            return foundPosition;
        }

        public async Task<PositionPlainDTO> AddAsync(PositionAddingDTO positionToAdd)
        {
            var addedPosition = await unitOfWork.PositionRepository.AddAsync(mapper.Map<Position>(positionToAdd));

            return mapper.Map<PositionPlainDTO>(addedPosition);
        }

        public async Task<PositionPlainDTO> UpdateAsync(PositionPlainDTO positionToUpdate)
        {
            var updatedPosition = await unitOfWork.PositionRepository.UpdateAsync(mapper.Map<Position>(positionToUpdate));

            if (updatedPosition is null)
                throw new NotFoundException("Position not found");

            return mapper.Map<PositionPlainDTO>(updatedPosition);
        }

        public async void DeleteAsync(int positionId)
        {
            var isDeleted = await unitOfWork.PositionRepository.DeleteAsync(positionId);

            if (!isDeleted)
                throw new NotFoundException("Position not found");

            return;
        }
    }
}
