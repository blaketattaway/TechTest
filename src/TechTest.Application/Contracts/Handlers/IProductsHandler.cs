using TechTest.Domain.DTOs.Products;
using TechTest.Domain.Entities;
using TechTest.Domain.DTOs.Tenants;

namespace TechTest.Application.Contracts.Handlers
{
    public interface IProductsHandler
    {
        Task<ResponseObject<List<ProductDTO>>> GetAll(TenantDTO tenant);
        Task<ResponseObject<ProductDTO>> GetById(TenantDTO tenant, int productId);
        Task<ResponseObject<bool>> Create(TenantDTO tenant, ProductDTO product);
        Task<ResponseObject<bool>> Update(TenantDTO tenant, ProductDTO product);
        Task<ResponseObject<bool>> Delete(TenantDTO tenant, int productId);
    }
}