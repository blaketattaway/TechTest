using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTest.Application.Contracts.Handlers;
using TechTest.Domain.DTOs.Login;

namespace TechTest.API.Controllers
{
    public class OrganizationsController : TechTestController
    {
        private readonly IOrganizationsHandler _organizationsHandler;

        public OrganizationsController(IOrganizationsHandler organizationsHandler)
        {
            _organizationsHandler = organizationsHandler;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(OrganizationDTO organization)
        {
            return Ok(await _organizationsHandler.Create(organization)) ;
        }
    }
}