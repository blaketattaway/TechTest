using Dapper;
using System.Data;
using TechTest.Application.Contracts.Repositories;
using TechTest.Domain.DTOs.Tenants;

namespace TechTest.Infrastructure.Repositories
{
    public class TenantsRepository : ITenantsRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;
        public TenantsRepository(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _dbConnection = dbTransaction.Connection!;
        }
        public async Task<TenantDTO?> GetByName(string slugName)
        {
            string spString = "[dbo].[Usp_Tenants_SEL]";

            return (await _dbConnection.QueryAsync<TenantDTO>(spString,new { pc_Name = slugName }, _dbTransaction,
                commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }

        public async Task Create(string slugName, string connectionString)
        {
            string spString = "[dbo].[Usp_Tenants_INS]";

            await _dbConnection.ExecuteAsync(spString, new { pc_Name = slugName, pc_ConnectionString = connectionString }, _dbTransaction, commandType: CommandType.StoredProcedure);
        }
    }
}