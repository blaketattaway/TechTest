using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTest.Application.Contracts.Handlers;
using TechTest.Domain.DTOs.Login;

namespace TechTest.API.Controllers
{
    public class UsersController : TechTestController
    {
        private readonly ILoginHandler _loginHandler;
        private readonly IUsersHandler _usersHandler;

        public UsersController(ILoginHandler loginHandler, IUsersHandler usersHandler)
        {
            _loginHandler = loginHandler;
            _usersHandler = usersHandler;

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDTO user)
        {
            return Ok(await _loginHandler.Login(user));
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO user)
        {
            return Ok(await _usersHandler.Create(user));
        }
    }
}