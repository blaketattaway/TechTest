using TechTest.Domain.DTOs.Login;
using TechTest.Domain.Entities;

namespace TechTest.Application.Contracts.Handlers
{
    public interface IOrganizationsHandler
    {
        Task<ResponseObject<bool>> Create(OrganizationDTO organization);

        Task<OrganizationDTO?> GetById(int organizationId);
    }
}