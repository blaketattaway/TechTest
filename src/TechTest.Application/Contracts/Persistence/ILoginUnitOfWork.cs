using TechTest.Application.Contracts.Repositories;

namespace TechTest.Application.Contracts.Persistence
{
    public interface ILoginUnitOfWork : IDisposable
    {
        public IOrganizationsRepository OrganizationsRepository { get; set; }
        public IUsersRepository UsersRepository { get; set; }
        void Commit();
    }
}