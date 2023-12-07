using Microsoft.AspNetCore.Http.Extensions;
using TechTest.API.Tenants;
using TechTest.Application.Contracts.Handlers;
using TechTest.Domain.Entities;

namespace TechTest.API.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITenantHandler _tenantHandler;

        public TenantMiddleware(RequestDelegate next, ITenantHandler tenantHandler)
        {
            _next = next;
            _tenantHandler = tenantHandler;
        }

        public async Task Invoke(HttpContext context, ITenantSetter tenantSetter, IConfiguration configuration)
        {
            try
            {
                context.Items["Tenant"] = null;

                var path = context.Request.Path.ToString();

                var splittedPath = path.Split('/');

                if (splittedPath.Length == 4 && splittedPath[2].Equals("products", StringComparison.OrdinalIgnoreCase))
                {
                    (string? tenantName, string? realPath) = GetTenantAndPathFrom(context.Request);

                    var currentTenant = await _tenantHandler.GetByName(splittedPath[1]);

                    if (currentTenant == null)
                    {
                        throw new Exception("Invalid request");
                    }
                    context.Request.PathBase = $"/{tenantName}";
                    context.Request.Path = realPath;

                    tenantSetter.CurrentTenant = currentTenant;
                }

                await _next.Invoke(context);
            }
            catch (Exception ex)
            {

            }
        }

        private static (string? tenantName, string? realPath)
        GetTenantAndPathFrom(HttpRequest httpRequest)
        {
            var tenantName = new Uri(httpRequest.GetDisplayUrl())
                .Segments
                .FirstOrDefault(x => x != "/")
                ?.TrimEnd('/');

            if (!string.IsNullOrWhiteSpace(tenantName) &&
                httpRequest.Path.StartsWithSegments($"/{tenantName}",
                    out PathString realPath))
            {
                return (tenantName, realPath);
            }

            return (null, null);
        }
    }
}
