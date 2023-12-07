namespace TechTest.Application.Contracts.Migrations
{
    public interface IProductsMigration
    {
        Task<bool> MigrateToNewDatabase(string slugTenant);
    }
}