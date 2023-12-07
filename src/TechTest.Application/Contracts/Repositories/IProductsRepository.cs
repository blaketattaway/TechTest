using TechTest.Domain.DTOs.Products;
using TechTest.Domain.DTOs.Tenants;

namespace TechTest.Application.Contracts.Repositories
{
    public interface IProductsRepository
    {
        Task<List<ProductDTO>> GetAll(TenantDTO tenant);
        Task<ProductDTO?> GetById(TenantDTO tenant, int productId);
        Task Create(TenantDTO tenant, ProductDTO product);
        Task Update(TenantDTO tenant, ProductDTO product);
        Task Delete(TenantDTO tenant, int productId);
    }
}