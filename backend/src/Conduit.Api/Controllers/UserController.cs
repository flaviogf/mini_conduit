using System.Threading.Tasks;
using Conduit.Api.Infrastructure;
using Conduit.Api.Models;
using Conduit.Api.Repositories;
using Conduit.Api.ViewModels;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ApplicationController
    {
        private readonly IUserRepository _userRepository;
        private readonly IHash _hash;
        private readonly IUnitOfWork _uow;

        public UserController(IUserRepository userRepository, IHash hash, IUnitOfWork uow)
        {
            _userRepository = userRepository;
            _hash = hash;
            _uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Store([FromBody] StoreUserRequest request)
        {
            Maybe<User> maybeUser = await _userRepository.FindByEmail(request.User.Email);

            if (maybeUser.HasValue)
            {
                return UnexpectedError(new ErrorResponse("This email is already taken"));
            }

            string passwordHash = await _hash.Make(request.User.Password);

            var user = new User
            {
                Username = request.User.Username,
                Email = request.User.Email,
                PasswordHash = passwordHash,
            };

            await _userRepository.Save(user);

            await _uow.Commit();

            return Created(new UserResponse(user));
        }
    }
}
