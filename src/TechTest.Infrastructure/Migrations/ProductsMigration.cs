using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using TechTest.Application.Contracts.Migrations;

namespace TechTest.Infrastructure.Migrations
{
    public class ProductsMigration : IProductsMigration
    {
		private readonly SqlConnection _masterConnection;

        public ProductsMigration(IConfiguration configuration)
        {
			_masterConnection = new SqlConnection(configuration.GetConnectionString("master"));
        }

        public async Task<bool> MigrateToNewDatabase(string slugTenant)
        {
            var success = true;

            var createLoginQuery = @$"CREATE LOGIN {slugTenant}_login WITH PASSWORD = '{slugTenant}_password';
                                      USE {slugTenant}
									  CREATE USER {slugTenant}_user FOR LOGIN {slugTenant}_login";

            var createtableQuery = @$"
							USE {slugTenant}

							CREATE TABLE products(
								ProductId INT IDENTITY PRIMARY KEY,
								Name VARCHAR(100) NOT NULL,
								Description VARCHAR(500),
							    Quantity INT NOT NULL)
	
							GRANT SELECT, INSERT, UPDATE, DELETE ON products TO {slugTenant}_user";
            try
            {
                await _masterConnection.OpenAsync();
                var createDatabaseCommand = new SqlCommand($"CREATE DATABASE {slugTenant}", _masterConnection);
                var createLoginCommand = new SqlCommand(createLoginQuery, _masterConnection);
                var createTableCommand = new SqlCommand(createtableQuery, _masterConnection);
                await createDatabaseCommand.ExecuteNonQueryAsync();
                await createLoginCommand.ExecuteNonQueryAsync();
                await createTableCommand.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                await _masterConnection.CloseAsync();
            }

            return success;
        }
    }
}