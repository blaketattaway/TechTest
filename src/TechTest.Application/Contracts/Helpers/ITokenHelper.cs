using TechTest.Domain.DTOs.Login;
using TechTest.Domain.Entities;

namespace TechTest.Application.Contracts.Helpers
{
    public interface ITokenHelper
    {
        LoginResponse Generate(UserDTO user);
    }
}