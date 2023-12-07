using TechTest.Domain.DTOs.Login;
using TechTest.Domain.Entities;

namespace TechTest.Application.Contracts.Handlers
{
    public interface IUsersHandler
    {
        Task<ResponseObject<bool>> Create(UserDTO user);
    }
}