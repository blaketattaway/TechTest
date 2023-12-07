namespace TechTest.Application
{
    #region Class Libraries
    using Microsoft.Extensions.DependencyInjection;
    using TechTest.Application.Contracts.Handlers;
    using TechTest.Application.Handlers;
    #endregion


    /// <summary>
    /// Registers Application for dependency injection
    /// </summary>
    public static class ServiceRegistration
    {
        #region Methods
        /// <summary>
        /// Adds Application Layer interface implementations to services
        /// </summary>
        /// <param name="services"></param>
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ILoginHandler, LoginHandler>();
            services.AddScoped<IUsersHandler, UsersHandler>();
            services.AddScoped<IOrganizationsHandler, OrganizationsHandler>();
            services.AddScoped<IProductsHandler, ProductsHandler>();
            services.AddTransient<ITenantHandler, TenantHandler>();
        }
        #endregion
    }
}