﻿using AutoMapper;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
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

        public async Task<List<DepartmentPlainDTO>> GetAllAsync()
        {
            var departmentsToMap = await unitOfWork.DepartmentRepository.GetAllAsync();

            return departmentsToMap.Select(mapper.Map<DepartmentPlainDTO>).ToList();
        }

        public async Task<DepartmentPlainDTO?> GetByIdAsync(int id)
        {
            return mapper.Map<DepartmentPlainDTO>(await unitOfWork.DepartmentRepository.GetByIdAsync(id));
        }

        public async Task<DepartmentDetailsDTO?> GetDetailsByIdAsync(int id)
        {
            return mapper.Map<DepartmentDetailsDTO>(await unitOfWork.DepartmentRepository.GetDetailsAsync(id));
        }

        public async Task<DepartmentPlainDTO> AddAsync(DepartmentAddingDTO departmentToAdd)
        {
            var addedDepartment = await unitOfWork.DepartmentRepository.AddAsync(mapper.Map<Department>(departmentToAdd));

            return mapper.Map<DepartmentPlainDTO>(addedDepartment);
        }

        public async Task<bool> AddWorkerToDepartmentAsync(DepartmentWorkerAddingDTO departmentWorkerAdding)
        {
            var departmentWorker = mapper.Map<DepartmentWorker>(departmentWorkerAdding);

            if (unitOfWork.DepartmentWorkerRepository.Exists(departmentWorker) 
                || await unitOfWork.WorkerRepository.GetByIdAsync(departmentWorker.WorkerId) is null
                || await unitOfWork.DepartmentRepository.GetByIdAsync(departmentWorker.DepartmentId) is null)
            {
                return false;
            }

            await unitOfWork.DepartmentWorkerRepository.AddAsync(departmentWorker);

            return true;
        }

        public async Task<DepartmentPlainDTO?> UpdateAsync(DepartmentPlainDTO departmentToUpdate)
        {
            var updatedDepartment = await unitOfWork.DepartmentRepository.UpdateAsync(mapper.Map<Department>(departmentToUpdate));

            return mapper.Map<DepartmentPlainDTO?>(updatedDepartment);
        }

        public async Task<bool> DeleteAsync(int departmentId)
        {
            return await unitOfWork.DepartmentRepository.DeleteAsync(departmentId);
        }
    }
}
