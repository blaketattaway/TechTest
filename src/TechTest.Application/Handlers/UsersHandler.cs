using TechTest.Application.Contracts.Handlers;
using TechTest.Application.Contracts.Persistence;
using TechTest.Domain.DTOs.Login;
using TechTest.Domain.Entities;

namespace TechTest.Application.Handlers
{
    public class UsersHandler : IUsersHandler
    {
        private readonly ILoginUnitOfWork _loginUnitOfWork;

        public UsersHandler(ILoginUnitOfWork loginUnitOfWork)
        {
            _loginUnitOfWork = loginUnitOfWork;
        }

        public async Task<ResponseObject<bool>> Create(UserDTO user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrEmpty(user.Password) || string.IsNullOrWhiteSpace(user.Password))
            {
                return new ResponseObject<bool>
                {
                    Status = 400,
                    StatusText = "Email and password must not be null or empty"
                };
            }

            if (user.OrganizationId == default)
            {
                return new ResponseObject<bool>
                {
                    Status = 400,
                    StatusText = "User must have associated with an organization"
                };
            }

            var currentUser = await _loginUnitOfWork.UsersRepository.GetByEmail(user.Email);

            if (currentUser != null)
            {
                return new ResponseObject<bool>
                {
                    Status = 400,
                    StatusText = "An user with that email is already registered"
                };
            }

            await _loginUnitOfWork.UsersRepository.Create(user);

            _loginUnitOfWork.Commit();

            return new ResponseObject<bool>();
        }
    }
}