using Dapper;
using System.Data;
using TechTest.Application.Contracts.Repositories;
using TechTest.Domain.DTOs.Login;

namespace TechTest.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;
        public UsersRepository(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _dbConnection = dbTransaction.Connection!;
        }

        public async Task Create(UserDTO user)
        {
            string spString = "[dbo].[Usp_Users_INS]";

            await _dbConnection.ExecuteAsync(spString, new { pc_Email = user.Email, pc_Password = user.Password, pi_OrganizationId = user.OrganizationId }, _dbTransaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<UserDTO?> GetByEmail(string email)
        {
            string spString = "[dbo].[Usp_UserByEmail]";

            return (await _dbConnection.QueryAsync<UserDTO>(spString, new { pc_Email = email }, _dbTransaction,
                commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }

        public async Task<bool> CheckPassword(UserDTO user)
        {
            string spString = "[dbo].[Usp_CheckPassword]";

            return (await _dbConnection.QueryAsync<bool>(spString, new { pc_Email = user.Email, pc_Password = user.Password }, _dbTransaction,
                commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }
    }
}