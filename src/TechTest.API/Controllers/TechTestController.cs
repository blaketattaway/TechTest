using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TechTest.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TechTestController : ControllerBase
    {
        private ClaimsIdentity? _identity;

        protected ClaimsIdentity Identity => _identity ??= (ClaimsIdentity)HttpContext.User.Identity!;

        protected string GetClaim(string claimName)
        {
            return Identity?.FindFirst(claimName)?.Value!;
        }
    }
}
