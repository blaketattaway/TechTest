using TechTest.API.Tenants;

namespace TechTest.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMultitenancy(this IServiceCollection services)
        {
            services.AddScoped<TenantContext>();

            services.AddScoped<ITenantContext>(provider =>
                provider.GetRequiredService<TenantContext>());

            services.AddScoped<ITenantSetter>(provider =>
                provider.GetRequiredService<TenantContext>());

            return services;
        }
    }
}