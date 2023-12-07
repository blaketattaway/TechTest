using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTest.API.Tenants;
using TechTest.Application.Contracts.Handlers;
using TechTest.Domain.DTOs.Products;

namespace TechTest.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsHandler _productsHandler;
        private readonly ITenantContext _tenantContext;

        public ProductsController(IProductsHandler productsHandler, ITenantContext tenantContext)
        {
            _productsHandler = productsHandler;
            _tenantContext = tenantContext;
        }

        [HttpGet("GetById/{productId}")]
        public async Task<IActionResult> GetById(int productId)
        {
            return Ok(await _productsHandler.GetById(_tenantContext.CurrentTenant, productId));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productsHandler.GetAll(_tenantContext.CurrentTenant));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProductDTO product)
        {
            return Ok(await _productsHandler.Create(_tenantContext.CurrentTenant, product));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProductDTO product)
        {
            return Ok(await _productsHandler.Update(_tenantContext.CurrentTenant, product));
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int productId)
        {
            return Ok(await _productsHandler.Delete(_tenantContext.CurrentTenant, productId));
        }
    }
}
