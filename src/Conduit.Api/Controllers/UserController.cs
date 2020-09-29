using System.Threading.Tasks;
using AutoMapper;
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
        private readonly UserManager<User> _userManager;

        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(User);

            return Ok(Res.Success(_mapper.Map<UserViewModel>(user)));
        }
    }
}
