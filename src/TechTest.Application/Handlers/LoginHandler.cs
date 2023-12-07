using TechTest.Application.Contracts.Handlers;
using TechTest.Application.Contracts.Helpers;
using TechTest.Application.Contracts.Persistence;
using TechTest.Domain.DTOs.Login;
using TechTest.Domain.Entities;

namespace TechTest.Application.Handlers
{
    public class LoginHandler : ILoginHandler
    {
        private readonly ILoginUnitOfWork _unitOfWork;
        private readonly ITokenHelper _tokenHelper;

        public LoginHandler(ILoginUnitOfWork unitOfWork, ITokenHelper tokenHelper)
        {
            _unitOfWork = unitOfWork;
            _tokenHelper = tokenHelper;
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

            var usersOrganization = await _unitOfWork.OrganizationsRepository.GetById(currentUser.OrganizationId);

            var loginResponse = _tokenHelper.Generate(user);

            loginResponse.Tenants = new List<Tenant> { new Tenant { SlugTenant = usersOrganization.SlugTenant }  };

            return new ResponseObject<LoginResponse>
            {
                Data = loginResponse
            };
        }
    }
}