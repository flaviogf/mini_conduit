using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Api.Infrastructure
{
    public class JwtTokenManager<T> : ITokenManager<T> where T : IdentityUser
    {
        private readonly IConfiguration _configuration;

        public JwtTokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> GenerateTokenAsync(T user)
        {
            var handler = new JwtSecurityTokenHandler();

            var expires = DateTime.UtcNow.AddHours(2);

            var subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id)
            });

            var secret = _configuration.GetValue<string>("JwtConfig:Secret");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                Expires = expires,
                SigningCredentials = credentials,
                Subject = subject,
            };

            var token = handler.CreateToken(descriptor);

            var result = handler.WriteToken(token);

            return Task.FromResult(result);
        }
    }
}
