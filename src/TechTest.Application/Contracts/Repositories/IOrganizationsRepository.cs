using TechTest.Domain.DTOs.Login;

namespace TechTest.Application.Contracts.Repositories
{
    public interface IOrganizationsRepository
    {
        Task<OrganizationDTO?> GetById(int id);
        Task Create(OrganizationDTO organization);
    }
}