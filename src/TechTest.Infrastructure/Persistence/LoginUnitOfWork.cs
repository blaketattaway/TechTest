using TechTest.Application.Contracts.Persistence;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using TechTest.Application.Contracts.Repositories;
using TechTest.Infrastructure.Repositories;
using System.Data;

namespace TechTest.Infrastructure.Persistence
{
    public class LoginUnitOfWork : ILoginUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;

        public LoginUnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new SqlConnection(_configuration.GetConnectionString("login")!);
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction();

            OrganizationsRepository = new OrganizationsRepository(_dbTransaction);
            UsersRepository = new UsersRepository(_dbTransaction);
        }

        public IOrganizationsRepository OrganizationsRepository { get; set; }
        public ITenantsRepository TenantsRepository { get; set; }
        public IUsersRepository UsersRepository { get; set; }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch (Exception)
            {
                _dbTransaction.Rollback();
            }
            finally
            {
                _dbTransaction.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbConnection?.Dispose();
            }
        }

        ~LoginUnitOfWork()
        {
            Dispose(false);
        }
    }
}