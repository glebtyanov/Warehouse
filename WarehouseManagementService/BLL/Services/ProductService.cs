using AutoMapper;
using BLL.DTO.Adding;
using BLL.DTO.Plain;
using DAL.Entities;
using DAL.UnitsOfWork;
using BLL.Exceptions;

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

        public async Task<ProductPlainDTO> GetByIdAsync(int id)
        {
            var foundProduct = mapper.Map<ProductPlainDTO>(await unitOfWork.ProductRepository.GetByIdAsync(id));

            if (foundProduct is null)
                throw new NotFoundException("Product not found");

            return foundProduct;
        }

        public async Task<ProductDetailsDTO> GetDetailsByIdAsync(int id)
        {
            var foundProduct = mapper.Map<ProductDetailsDTO>(await unitOfWork.ProductRepository.GetDetailsAsync(id));

            if (foundProduct is null)
                throw new NotFoundException("Product not found");

            return foundProduct;
        }

        public async Task<ProductPlainDTO> AddAsync(ProductAddingDTO productToAdd)
        {
            var addedProduct = await unitOfWork.ProductRepository.AddAsync(mapper.Map<Product>(productToAdd));

            return mapper.Map<ProductPlainDTO>(addedProduct);
        }

        public async Task<ProductPlainDTO> UpdateAsync(ProductPlainDTO productToUpdate)
        {
            var updatedProduct = await unitOfWork.ProductRepository.UpdateAsync(mapper.Map<Product>(productToUpdate));

            if (updatedProduct is null)
                throw new NotFoundException("Product not found");

            return mapper.Map<ProductPlainDTO>(updatedProduct);
        }

        public async void DeleteAsync(int productId)
        {
            var isDeleted = await unitOfWork.ProductRepository.DeleteAsync(productId);

            if(!isDeleted)
                throw new NotFoundException("Product not found");

            return;
        }
    }
}
