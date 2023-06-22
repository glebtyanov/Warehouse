using AutoMapper;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
using BLL.Exceptions;
using DAL.Entities;
using DAL.UnitsOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<List<DepartmentPlainDTO>> GetAllAsync()
        {
            var departmentsToMap = await unitOfWork.DepartmentRepository.GetAllAsync();

            return departmentsToMap.Select(mapper.Map<DepartmentPlainDTO>).ToList();
        }

        public async Task<DepartmentPlainDTO> GetByIdAsync(int id)
        {
            var foundDepartment = await unitOfWork.DepartmentRepository.GetByIdAsync(id);

            if (foundDepartment is null)
                throw new NotFoundException("Department not found");

            return mapper.Map<DepartmentPlainDTO>(foundDepartment);
        }

        public async Task<DepartmentDetailsDTO> GetDetailsByIdAsync(int id)
        {
            var foundDepartment = await unitOfWork.DepartmentRepository.GetDetailsAsync(id);

            if (foundDepartment is null)
                throw new NotFoundException("Department not found");

            return mapper.Map<DepartmentDetailsDTO>(foundDepartment);
        }

        public async Task<DepartmentPlainDTO> AddAsync(DepartmentAddingDTO departmentToAdd)
        {
            var addedDepartment = await unitOfWork.DepartmentRepository.AddAsync(mapper.Map<Department>(departmentToAdd));

            return mapper.Map<DepartmentPlainDTO>(addedDepartment);
        }

        public async void AddWorkerToDepartmentAsync(DepartmentWorkerAddingDTO departmentWorkerAdding)
        {
            var departmentWorker = mapper.Map<DepartmentWorker>(departmentWorkerAdding);

            if (unitOfWork.DepartmentWorkerRepository.Exists(departmentWorker))
                throw new AlreadyExistsException("Worker is already in department");

            if (await unitOfWork.WorkerRepository.GetByIdAsync(departmentWorker.WorkerId) is null
                || await unitOfWork.DepartmentRepository.GetByIdAsync(departmentWorker.DepartmentId) is null)
                throw new NotFoundException("Department or worker not found");

            await unitOfWork.DepartmentWorkerRepository.AddAsync(departmentWorker);
            return;
        }

        public async Task<DepartmentPlainDTO> UpdateAsync(DepartmentPlainDTO departmentToUpdate)
        {
            var updatedDepartment = await unitOfWork.DepartmentRepository.UpdateAsync(mapper.Map<Department>(departmentToUpdate));

            if (updatedDepartment is null)
                throw new NotFoundException("Department not found");

            return mapper.Map<DepartmentPlainDTO>(updatedDepartment);
        }

        public async void DeleteAsync(int departmentId)
        {
            var isDeleted = await unitOfWork.DepartmentRepository.DeleteAsync(departmentId);

            if (!isDeleted)
                throw new NotFoundException("Department not found");

            return;
        }
    }
}
