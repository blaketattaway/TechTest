using TechTest.Domain.DTOs.Login;
using TechTest.Domain.Entities;

namespace TechTest.Application.Contracts.Handlers
{
    public interface ILoginHandler
    {
        Task<ResponseObject<LoginResponse>> Login(UserDTO user);
    }
}