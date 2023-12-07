using TechTest.Domain.DTOs.Login;

namespace TechTest.Application.Contracts.Repositories
{
    public interface IUsersRepository
    {
        Task Create(UserDTO user);
        Task<UserDTO?> GetByEmail(string email);
        Task<bool> CheckPassword(UserDTO user);
    }
}