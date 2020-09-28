using System.Threading.Tasks;
using Conduit.Api.Infrastructure;
using Conduit.Api.Models;
using Conduit.Api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Res = Conduit.Api.Infrastructure.Response;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Route("api/sign-in")]
    public class SignInController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;

        private readonly UserManager<User> _userManager;

        private readonly ITokenManager<User> _tokenManager;

        public SignInController(SignInManager<User> signInManager, UserManager<User> userManager, ITokenManager<User> tokenManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SignInRequest req)
        {
            User user = await _userManager.FindByEmailAsync(req.Email);

            if (user == null)
            {
                return Unauthorized(Res.Failure("Wrong email or password."));
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);

            if (!signInResult.Succeeded)
            {
                return Unauthorized(Res.Failure("Wrong email or password."));
            }

            var token = await _tokenManager.GenerateTokenAsync(user);

            return Ok(Res.Success(token));
        }
    }
}
