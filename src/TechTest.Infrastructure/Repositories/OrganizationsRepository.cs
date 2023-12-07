using Dapper;
using System.Data;
using TechTest.Application.Contracts.Repositories;
using TechTest.Domain.DTOs.Login;

namespace TechTest.Infrastructure.Repositories
{
    public class OrganizationsRepository : IOrganizationsRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;
        public OrganizationsRepository(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _dbConnection = dbTransaction.Connection!;
        }

        public async Task<OrganizationDTO?> GetById(int id)
        {
            string spString = "[dbo].[Usp_Organizations_SEL]";

            return (await _dbConnection.QueryAsync<OrganizationDTO>(spString, new { pi_OrganizationId = id }, _dbTransaction,
                commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }

        public async Task Create(OrganizationDTO organization)
        {
            string spString = "[dbo].[Usp_Organizations_INS]";
            await _dbConnection.ExecuteAsync(spString, new { pc_Name = organization.Name, pc_SlugTenant = organization.SlugTenant }, _dbTransaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}