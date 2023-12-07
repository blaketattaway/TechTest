using TechTest.Application.Contracts.Handlers;
using TechTest.Application.Contracts.Repositories;
using TechTest.Domain.DTOs.Products;
using TechTest.Domain.DTOs.Tenants;
using TechTest.Domain.Entities;

namespace TechTest.Application.Handlers
{
    public class ProductsHandler : IProductsHandler
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsHandler(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<ResponseObject<bool>> Create(TenantDTO tenant, ProductDTO product)
        {
            if (string.IsNullOrEmpty(product.Name) || string.IsNullOrWhiteSpace(product.Name))
            {
                return new()
                {
                    Status = 400,
                    StatusText = "Product name cannot be null or empty"
                };
            }

            await _productsRepository.Create(tenant, product);

            return new();
        }

        public async Task<ResponseObject<bool>> Delete(TenantDTO tenant, int productId)
        {
            var currentProduct = await _productsRepository.GetById(tenant, productId);

            if (currentProduct == null)
            {
                return new ResponseObject<bool>
                {
                    Status = 404,
                    StatusText = "Product not found"
                };
            }

            await _productsRepository.Delete(tenant, productId);

            return new();
        }

        public async Task<ResponseObject<List<ProductDTO>>> GetAll(TenantDTO tenant)
        {
            return new()
            {
                Data = await _productsRepository.GetAll(tenant)
            };
        }

        public async Task<ResponseObject<ProductDTO>> GetById(TenantDTO tenant, int productId)
        {
            return new()
            {
                Data = await _productsRepository.GetById(tenant, productId)
            };
        }

        public async Task<ResponseObject<bool>> Update(TenantDTO tenant, ProductDTO product)
        {

            var currentProduct = await _productsRepository.GetById(tenant, product.ProductId);

            if (currentProduct == null)
            {
                return new ResponseObject<bool>
                {
                    Status = 404,
                    StatusText = "Product not found"
                };
            }

            await _productsRepository.Update(tenant, product);

            return new();
        }
    }
}