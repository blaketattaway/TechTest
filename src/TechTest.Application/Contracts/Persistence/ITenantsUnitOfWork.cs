using TechTest.Application.Contracts.Repositories;

namespace TechTest.Application.Contracts.Persistence
{
    public interface ITenantsUnitOfWork : IDisposable
    {
        public ITenantsRepository TenantsRepository { get; set; }
        void Commit();
    }
}