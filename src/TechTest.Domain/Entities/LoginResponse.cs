namespace TechTest.Domain.Entities
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public List<Tenant> Tenants { get; set; }
    }
}