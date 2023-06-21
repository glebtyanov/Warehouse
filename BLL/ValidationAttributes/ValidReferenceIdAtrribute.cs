using System;
using System.ComponentModel.DataAnnotations;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.UnitsOfWork;

namespace BLL.ValidationAttributes
{
    // validation attribute for checking whether there is record with provided foreign key id 
    public class ValidReferenceIdAttribute<Entity> : ValidationAttribute where Entity : class
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int id = (int)value;

            // first get unitOfWork from the runtime
            var unitOfWork = (IUnitOfWork)validationContext.GetService(typeof(IUnitOfWork));

            // then get type of repository for given entity
            var repositoryType = typeof(IBaseRepository<Entity>);

            var repositoryProperty = unitOfWork.GetType().GetProperty(nameof(repositoryType));

            if (repositoryProperty is null)
            {
                throw new ArgumentException($"Invalid model property name: {nameof(Entity)}");
            }

            var repository = (IBaseRepository<Entity>)repositoryProperty.GetValue(unitOfWork);

            if (repository.GetByIdAsync(id).Result == null)
            {
                return new ValidationResult($"Invalid {validationContext.DisplayName} Id");
            }

            return ValidationResult.Success;
        }
    }
}
