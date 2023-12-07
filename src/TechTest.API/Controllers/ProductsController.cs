using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechTest.API.Tenants;
using TechTest.Application.Contracts.Handlers;
using TechTest.Domain.DTOs.Products;
using static Dapper.SqlMapper;

namespace TechTest.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsHandler _productsHandler;
        private readonly ITenantContext _tenantContext;
        private readonly IOrganizationsHandler _organizationsHandler;
        private ClaimsIdentity? _identity;
        private ClaimsIdentity Identity => _identity ??= (ClaimsIdentity)HttpContext.User.Identity!;

        public ProductsController(IProductsHandler productsHandler, IOrganizationsHandler organizationsHandler, ITenantContext tenantContext)
        {
            _productsHandler = productsHandler;
            _tenantContext = tenantContext;
            _organizationsHandler = organizationsHandler;
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery]int productId)
        {
            return Ok(await _productsHandler.GetById(_tenantContext.CurrentTenant, productId));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            await ValidateToken();
            return Ok(await _productsHandler.GetAll(_tenantContext.CurrentTenant));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProductDTO product)
        {
            await ValidateToken();
            return Ok(await _productsHandler.Create(_tenantContext.CurrentTenant, product));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProductDTO product)
        {
            await ValidateToken();
            return Ok(await _productsHandler.Update(_tenantContext.CurrentTenant, product));
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromQuery]int productId)
        {
            await ValidateToken();
            return Ok(await _productsHandler.Delete(_tenantContext.CurrentTenant, productId));
        }

        private async Task<bool> ValidateToken()
        {
            var usersOrganization = GetClaim("Organization");

            if (int.TryParse(usersOrganization, out int organizationId))
            {
                var currentOrganization = await _organizationsHandler.GetById(organizationId);

                var valid = currentOrganization != null && currentOrganization.SlugTenant == _tenantContext.CurrentTenant.Name;

                if (!valid)
                {
                    throw new Exception("Unauthorized");
                }

                return valid;
            }
            else
            {
                return false;
            }
        }

        protected string GetClaim(string claimName)
        {
            return Identity?.FindFirst(claimName)?.Value!;
        }
    }
}
