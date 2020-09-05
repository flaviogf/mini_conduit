using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Conduit.Api.Models;
using Conduit.Api.Repositories;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Api.Infrastructure
{
    public class Jwt : IAuth
    {
        private readonly IUserRepository _userRepository;
        private readonly IHash _hash;
        private readonly IConfiguration _configuration;

        public Jwt(IUserRepository userRepository, IHash hash, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _hash = hash;
            _configuration = configuration;
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

            var handler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Secret"));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = handler.CreateToken(descriptor);

            user.Token = handler.WriteToken(token);

            return user;
        }
    }
}
