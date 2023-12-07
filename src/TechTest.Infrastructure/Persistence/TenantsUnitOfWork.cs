using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using TechTest.Application.Contracts.Persistence;
using TechTest.Application.Contracts.Repositories;
using TechTest.Infrastructure.Repositories;

namespace TechTest.Infrastructure.Persistence
{
    public class TenantsUnitOfWork : ITenantsUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;

        public TenantsUnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new SqlConnection(_configuration.GetConnectionString("tenants")!);
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction();

            TenantsRepository = new TenantsRepository(_dbTransaction);
        }

        public ITenantsRepository TenantsRepository { get; set; }

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

        ~TenantsUnitOfWork()
        {
            Dispose(false);
        }
    }
}