using TechTest.Domain.DTOs.Tenants;

namespace TechTest.Application.Contracts.Repositories
{
    public interface ITenantsRepository
    {
        Task<TenantDTO?> GetByName(string slugName);
        Task Create(string slugName, string connectionString);
    }
}