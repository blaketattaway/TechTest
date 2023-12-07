using TechTest.Application.Contracts.Handlers;
using TechTest.Application.Contracts.Persistence;
using TechTest.Domain.DTOs.Tenants;

namespace TechTest.Application.Handlers
{
    public class TenantHandler : ITenantHandler
    {
        private readonly ITenantsUnitOfWork _tenantsUnitOfWork;

        public TenantHandler(ITenantsUnitOfWork tenantsUnitOfWork)
        {
            _tenantsUnitOfWork = tenantsUnitOfWork;
        }

        public async Task<TenantDTO?> GetByName(string tenantName)
        {
            return await _tenantsUnitOfWork.TenantsRepository.GetByName(tenantName);
        }
    }
}