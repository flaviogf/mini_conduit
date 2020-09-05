using System;
using System.Threading.Tasks;
using Conduit.Api.Infrastructure;
using Conduit.Api.Models;
using Conduit.Api.Repositories;
using Conduit.Api.ViewModels;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IAuth _auth;

        public UserController(IUserRepository userRepository, IHash hash, IUnitOfWork uow, IAuth auth)
        {
            _userRepository = userRepository;
            _hash = hash;
            _uow = uow;
            _auth = auth;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await _auth.GetUser();

            return Ok(new UserResponse(user));
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
                Id = Guid.NewGuid().ToString(),
                Username = request.User.Username,
                Email = request.User.Email,
                PasswordHash = passwordHash,
            };

            await _userRepository.Save(user);

            await _uow.Commit();

            return Created(new UserResponse(user));
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditUserRequest request)
        {
            User user = await _auth.GetUser();

            Maybe<User> maybeUser = await _userRepository.FindByEmail(request.User.Email);

            if (maybeUser.HasValue && user.Id != maybeUser.Value.Id)
            {
                return UnexpectedError(new ErrorResponse("This email is already taken."));
            }

            user.Username = request.User.Username;
            user.Email = request.User.Email;
            user.Bio = request.User.Bio;
            user.Image = request.User.Image;

            if (!string.IsNullOrWhiteSpace(request.User.Password))
            {
                user.PasswordHash = await _hash.Make(request.User.Password);
            }

            await _uow.Commit();

            return Ok(new UserResponse(user));
        }
    }
}
