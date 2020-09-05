using System.Threading.Tasks;
using Conduit.Api.Infrastructure;
using Conduit.Api.Models;
using Conduit.Api.ViewModels;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Route("users/login")]
    public class UserLoginController : ApplicationController
    {
        private readonly IAuth _auth;

        public UserLoginController(IAuth auth)
        {
            _auth = auth;
        }

        [HttpPost]
        public async Task<IActionResult> Store([FromBody] StoreUserLoginRequest request)
        {
            Result<User> result = await _auth.Attempt(request.User.Email, request.User.Password);

            if (result.IsFailure)
            {
                return UnexpectedError(new ErrorResponse(result.Error));
            }

            User user = result.Value;

            return Ok(new UserResponse(user));
        }
    }
}
