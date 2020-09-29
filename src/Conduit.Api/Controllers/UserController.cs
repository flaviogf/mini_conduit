using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.Database;
using Conduit.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Res = Conduit.Api.Infrastructure.Response;

namespace Conduit.Api.ViewModels
{
    [ApiController]
    [Authorize]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ConduitDbContext _context;

        private readonly UserManager<User> _userManager;

        private readonly IMapper _mapper;

        public UserController(ConduitDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(User);

            return Ok(Res.Success(_mapper.Map<UserViewModel>(user)));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateUserRequest req)
        {
            var user = await _userManager.GetUserAsync(User);

            IdentityResult result = await _userManager.SetUserNameAsync(user, req.UserName);

            if (!result.Succeeded)
            {
                return UnprocessableEntity(Res.Failure(result.Errors.Select(it => it.Description)));
            }

            result = await _userManager.SetEmailAsync(user, req.Email);

            if (!result.Succeeded)
            {
                return UnprocessableEntity(Res.Failure(result.Errors.Select(it => it.Description)));
            }

            if (string.IsNullOrEmpty(req.CurrentPassword))
            {
                user.Avatar = req.Avatar;

                user.Bio = req.Bios;

                await _context.SaveChangesAsync();

                return Ok(Res.Success(user.Id));
            }

            result = await _userManager.ChangePasswordAsync(user, req.CurrentPassword, req.NewPassword);

            if (!result.Succeeded)
            {
                return UnprocessableEntity(Res.Failure(result.Errors.Select(it => it.Description)));
            }

            user.Avatar = req.Avatar;

            user.Bio = req.Bios;

            await _context.SaveChangesAsync();

            return Ok(Res.Success(user.Id));
        }
    }
}
