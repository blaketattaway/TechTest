using TechTest.Application.Contracts.Handlers;
using TechTest.Application.Contracts.Migrations;
using TechTest.Application.Contracts.Persistence;
using TechTest.Domain.DTOs.Login;
using TechTest.Domain.Entities;

namespace TechTest.Application.Handlers
{
    public class OrganizationsHandler : IOrganizationsHandler
    {
        private readonly ILoginUnitOfWork _loginUnitOfWork;
        private readonly ITenantsUnitOfWork _tenantsUnitOfWork;
        private readonly IProductsMigration _productsMigration;
        private const string CONNECTION_STRING_TEMPLATE = "Server=DESKTOP-GPD86LJ;Database=#databaseName;User Id=#databaseLogin_login;Password=#databasePassword_password;Trusted_Connection=False;MultipleActiveResultSets=True;";

        public OrganizationsHandler(ILoginUnitOfWork loginUnitOfWork, ITenantsUnitOfWork tenantsUnitOfWork, IProductsMigration productsMigration)
        {
            _loginUnitOfWork = loginUnitOfWork;
            _tenantsUnitOfWork = tenantsUnitOfWork;
            _productsMigration = productsMigration;
        }

        public async Task<ResponseObject<bool>> Create(OrganizationDTO organization)
        {
            if (string.IsNullOrEmpty(organization.Name) || string.IsNullOrEmpty(organization.SlugTenant))
            {
                return new ResponseObject<bool>
                {
                    Status = 400,
                    StatusText = "Organization's Name or SlugTenant can't be null or empty"
                };
            }

            var tenant = await _tenantsUnitOfWork.TenantsRepository.GetByName(organization.SlugTenant);

            if (tenant != null)
            {
                return new ResponseObject<bool>
                {
                    Status = 400,
                    StatusText = "SlugTenant is already taken"
                };
            }

            string currentConnection = CONNECTION_STRING_TEMPLATE.Replace("#databaseName", organization.SlugTenant).Replace("#databaseLogin", organization.SlugTenant).Replace("#databasePassword", organization.SlugTenant);

            var migratedSuccessFully = await _productsMigration.MigrateToNewDatabase(organization.SlugTenant);

            if (!migratedSuccessFully)
            {
                return new ResponseObject<bool>
                {
                    Status = 500,
                    StatusText = "There was an unexpected error"
                };
            }

            await _tenantsUnitOfWork.TenantsRepository.Create(organization.SlugTenant, currentConnection);

            await _loginUnitOfWork.OrganizationsRepository.Create(organization);

            _tenantsUnitOfWork.Commit();
            _loginUnitOfWork.Commit();

            return new ResponseObject<bool>();
        }
    }
}