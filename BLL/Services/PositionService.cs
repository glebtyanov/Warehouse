using AutoMapper;
using BLL.DTO.Adding;
using DAL.Entities;
using DAL.UnitsOfWork;
using BLL.DTO.Plain;

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

        public async Task<PositionPlainDTO?> GetByIdAsync(int id)
        {
            return mapper.Map<PositionPlainDTO>(await unitOfWork.PositionRepository.GetByIdAsync(id));
        }

        public async Task<PositionPlainDTO> AddAsync(PositionAddingDTO positionToAdd)
        {
            var addedPosition = await unitOfWork.PositionRepository.AddAsync(mapper.Map<Position>(positionToAdd));

            return mapper.Map<PositionPlainDTO>(addedPosition);
        }

        public async Task<PositionPlainDTO?> UpdateAsync(PositionPlainDTO positionToUpdate)
        {
            var updatedPosition = await unitOfWork.PositionRepository.UpdateAsync(mapper.Map<Position>(positionToUpdate));

            return mapper.Map<PositionPlainDTO?>(updatedPosition);
        }

        public async Task<bool> DeleteAsync(int positionId)
        {
            return await unitOfWork.PositionRepository.DeleteAsync(positionId);
        }
    }
}
