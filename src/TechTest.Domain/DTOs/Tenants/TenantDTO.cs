namespace TechTest.Domain.DTOs.Tenants
{
    public class TenantDTO
    {
        public int TenantId { get; set; }
        public string? Name { get; set; }
        public string? Identifier { get; set; }
        public string? ConnectionString { get; set; }
    }
}