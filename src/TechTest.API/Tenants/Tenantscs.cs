using TechTest.Domain.DTOs.Tenants;

namespace TechTest.API.Tenants
{
    public interface ITenantContext
    {
        TenantDTO CurrentTenant { get; }
    }

    public interface ITenantSetter
    {
        TenantDTO CurrentTenant { set; }
    }

    public class TenantContext : ITenantContext, ITenantSetter
    {
        public TenantDTO CurrentTenant { get; set; }
    }

    public class Tenant
    {
        public string ConnectionString { get; set; }

        public Tenant(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
