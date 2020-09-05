using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Conduit.Api.Models;
using Conduit.Api.Repositories;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Api.Infrastructure
{
    public class Jwt : IAuth
    {
        private readonly IUserRepository _userRepository;
        private readonly IHash _hash;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Jwt(IUserRepository userRepository, IHash hash, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _hash = hash;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<User>> Attempt(string email, string password)
        {
            Maybe<User> maybeUser = await _userRepository.FindByEmail(email);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<User>("Wrong email or password.");
            }

            User user = maybeUser.Value;

            if (!(await _hash.Verify(password, user.PasswordHash)))
            {
                return Result.Failure<User>("Wrong email or password.");
            }

            user.Token = TokenFor(user);

            return user;
        }

        public async Task<User> GetUser()
        {
            string id = _httpContextAccessor.HttpContext.User.Identity.Name;

            Maybe<User> maybeUser = await _userRepository.Find(id);

            if (maybeUser.HasNoValue)
            {
                throw new InvalidOperationException("Logged-in user cannot be found.");
            }

            User user = maybeUser.Value;

            user.Token = TokenFor(user);

            return user;
        }

        private string TokenFor(User user)
        {
            var handler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Secret"));

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id)
            };

            var descriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1)
            };

            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
