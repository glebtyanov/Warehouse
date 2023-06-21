using AutoMapper;
using BLL.DTO.Adding;
using DAL.Entities;
using DAL.UnitsOfWork;
using BLL.DTO.Plain;

namespace BLL.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<ProductPlainDTO>> GetAllAsync()
        {
            var productsToMap = await unitOfWork.ProductRepository.GetAllAsync();

            return productsToMap.Select(mapper.Map<ProductPlainDTO>).ToList();
        }

        public async Task<ProductPlainDTO?> GetByIdAsync(int id)
        {
            return mapper.Map<ProductPlainDTO>(await unitOfWork.ProductRepository.GetByIdAsync(id));
        }

        public async Task<ProductDetailsDTO?> GetDetailsByIdAsync(int id)
        {
            return mapper.Map<ProductDetailsDTO>(await unitOfWork.ProductRepository.GetDetailsAsync(id));
        }

        public async Task<ProductPlainDTO> AddAsync(ProductAddingDTO productToAdd)
        {
            var addedProduct = await unitOfWork.ProductRepository.AddAsync(mapper.Map<Product>(productToAdd));

            return mapper.Map<ProductPlainDTO>(addedProduct);
        }

        public async Task<ProductPlainDTO?> UpdateAsync(ProductPlainDTO productToUpdate)
        {
            var updatedProduct = await unitOfWork.ProductRepository.UpdateAsync(mapper.Map<Product>(productToUpdate));

            return mapper.Map<ProductPlainDTO?>(updatedProduct);
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            return await unitOfWork.ProductRepository.DeleteAsync(productId);
        }
    }
}
