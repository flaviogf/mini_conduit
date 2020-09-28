using System.Linq;
using System.Threading.Tasks;
using Conduit.Api.Models;
using Conduit.Api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Res = Conduit.Api.Infrastructure.Response;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Route("api/sign-up")]
    public class SignUpController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public SignUpController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SignUpRequest req)
        {
            var user = new User
            {
                UserName = req.UserName,
                Email = req.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, req.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(it => it.Description);

                return UnprocessableEntity(Res.Failure(errors));
            }

            return Created("/user", Res.Success(user.Id));
        }
    }
}
