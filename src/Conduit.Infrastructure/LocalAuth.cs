using System.Security.Claims;
using System.Threading.Tasks;
using Conduit.Application;
using Conduit.Domain.Users;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace Conduit.Infrastructure
{
    public class LocalAuth : IAuth
    {
        private readonly IUserRepository _userRepository;

        private readonly HttpContext _context;

        public LocalAuth(IUserRepository userRepository, IHttpContextAccessor contextAccessor)
        {
            _userRepository = userRepository;
            _context = contextAccessor.HttpContext;
        }

        public Task<Maybe<User>> GetUser()
        {
            var userId = _context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return _userRepository.Find(userId);
        }
    }
}
