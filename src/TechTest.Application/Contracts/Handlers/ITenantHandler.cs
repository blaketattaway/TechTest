using TechTest.Domain.DTOs.Tenants;

namespace TechTest.Application.Contracts.Handlers
{
    public interface ITenantHandler
    {
        Task<TenantDTO?> GetByName(string tenantName);
    }
}