using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechTest.Domain.DTOs.Login;
using TechTest.Domain.Entities;
using TechTest.Application.Contracts.Helpers;

namespace TechTest.Infrastructure.Helpers
{
    public class TokenHelper : ITokenHelper
    {
        private readonly IConfiguration _configuration;

        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LoginResponse Generate(UserDTO user)
        {
            var claims = new List<Claim>() {
                new Claim("User", user.Email),
                new Claim("Organization", user.OrganizationId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiration, signingCredentials: creds);

            return new LoginResponse()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}