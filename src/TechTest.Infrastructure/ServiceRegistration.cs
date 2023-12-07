using Microsoft.Extensions.DependencyInjection;
using TechTest.Application.Contracts.Helpers;
using TechTest.Application.Contracts.Migrations;
using TechTest.Application.Contracts.Persistence;
using TechTest.Application.Contracts.Repositories;
using TechTest.Infrastructure.Helpers;
using TechTest.Infrastructure.Migrations;
using TechTest.Infrastructure.Persistence;
using TechTest.Infrastructure.Repositories;

namespace TechTest.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ILoginUnitOfWork, LoginUnitOfWork>();
            services.AddTransient<ITenantsUnitOfWork, TenantsUnitOfWork>();
            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddTransient<IProductsMigration, ProductsMigration>();
            services.AddScoped<ITokenHelper, TokenHelper>();
        }
    }
}