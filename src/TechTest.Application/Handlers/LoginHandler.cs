using TechTest.Application.Contracts.Handlers;
using TechTest.Application.Contracts.Persistence;
using TechTest.Domain.DTOs.Login;
using TechTest.Domain.Entities;

namespace TechTest.Application.Handlers
{
    public class LoginHandler : ILoginHandler
    {
        private readonly ILoginUnitOfWork _unitOfWork;

        public LoginHandler(ILoginUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseObject<LoginResponse>> Login(UserDTO user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return new ResponseObject<LoginResponse>
                {
                    Status = 400,
                    StatusText = "Email and Password must not be empty"
                };
            }

            var currentUser = await _unitOfWork.UsersRepository.GetByEmail(user.Email);

            if (currentUser == null)
            {
                return new ResponseObject<LoginResponse>
                {
                    Status = 400,
                    StatusText = "User doesn't exists"
                };
            }

            var passwordMatch = await _unitOfWork.UsersRepository.CheckPassword(user);

            if (!passwordMatch)
            {
                return new ResponseObject<LoginResponse>
                {
                    Status = 400,
                    StatusText = "Bad Credentials"
                };
            }

            return new ResponseObject<LoginResponse>
            {

            };
        }
    }
}