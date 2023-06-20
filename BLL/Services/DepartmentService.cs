using AutoMapper;
using BLL.DTO;
using BLL.DTO.Adding;
using DAL.Entities;
using DAL.UnitsOfWork;

namespace BLL.Services
{
    public class DepartmentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<DepartmentDTO>> GetAllAsync()
        {
            var departmentsToMap = await unitOfWork.DepartmentRepository.GetAllAsync();

            return departmentsToMap.Select(mapper.Map<DepartmentDTO>).ToList();
        }

        public async Task<DepartmentDTO?> GetByIdAsync(int id)
        {
            return mapper.Map<DepartmentDTO>(await unitOfWork.DepartmentRepository.GetByIdAsync(id));
        }

        public async Task<DepartmentDTO> AddAsync(DepartmentAddingDTO departmentToAdd)
        {
            var addedDepartment = await unitOfWork.DepartmentRepository.AddAsync(mapper.Map<Department>(departmentToAdd));

            return mapper.Map<DepartmentDTO>(addedDepartment);
        }

        public async Task<DepartmentDTO?> UpdateAsync(DepartmentDTO departmentToUpdate)
        {
            var updatedDepartment = await unitOfWork.DepartmentRepository.UpdateAsync(mapper.Map<Department>(departmentToUpdate));

            return mapper.Map<DepartmentDTO?>(updatedDepartment);
        }

        public async Task<bool> DeleteAsync(int departmentId)
        {
            return await unitOfWork.DepartmentRepository.DeleteAsync(departmentId);
        }
    }
}
