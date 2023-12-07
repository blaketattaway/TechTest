using Dapper;
using System.Data.SqlClient;
using TechTest.Application.Contracts.Repositories;
using TechTest.Domain.DTOs.Products;
using TechTest.Domain.DTOs.Tenants;

namespace TechTest.Infrastructure.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        public async Task Create(TenantDTO tenant, ProductDTO product)
        {
            await Execute(tenant.ConnectionString!,
                @"INSERT 
                    INTO products
                  VALUES (@pc_Name, 
                  		@pc_Description,
                  		@pi_Quantity)", 
                new { pc_Name = product.Name, pc_Description = product.Description, pi_Quantity = product.Quantity });
        }

        public async Task Delete(TenantDTO tenant, int productId)
        {
            await Execute(tenant.ConnectionString!,
                @"DELETE
                    FROM products
                   WHERE ProductId = @pi_ProductId",
                new { pi_ProductId = productId });
        }

        public async Task<List<ProductDTO>> GetAll(TenantDTO tenant)
        {
            string spString = @"SELECT ProductId,
									   Name,
									   Description,
									   Quantity
								  FROM products";
            SqlConnection sqlConnection = new SqlConnection(tenant.ConnectionString);
            try
            {
                sqlConnection.Open();
                return (await sqlConnection.QueryAsync<ProductDTO>(spString)).ToList();
            }
            catch (Exception ex)
            {
                return new List<ProductDTO>();
            }
            finally
            {
                sqlConnection.Close();
            };
        }

        public async Task<ProductDTO?> GetById(TenantDTO tenant, int productId)
        {
            string spString = @"SELECT ProductId,
									   Name,
									   Description,
									   Quantity
								  FROM products
							     WHERE ProductId = @pi_ProductId";
            SqlConnection sqlConnection = new SqlConnection(tenant.ConnectionString);
            try
            {
                sqlConnection.Open();
                return (await sqlConnection.QueryAsync<ProductDTO>(spString, new { pi_ProductId = productId })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlConnection.Close();
            };
        }

        public async Task Update(TenantDTO tenant, ProductDTO product)
        {
            await Execute(tenant.ConnectionString!,
                @"UPDATE products
                     SET Name = @pc_Name,
                   	   Description = @pc_Description,
                   	   Quantity = @pi_Quantity
                   WHERE ProductId = @pi_ProductId",
                new { pi_ProductId = product.ProductId,  pc_Name = product.Name, pc_Description = product.Description, pi_Quantity = product.Quantity });
        }

        private async Task Execute(string connectionString, string spString, object obj)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
                await sqlConnection.ExecuteAsync(spString, obj);
            }
            catch (Exception ex)
            {
                throw new Exception("There was an unexpected error");
            }
            finally
            {
                sqlConnection.Close();
            };
        }
    }
}