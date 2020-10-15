using System.Threading.Tasks;
using Conduit.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Presentation.Api
{
    [ApiController]
    [Authorize]
    [Route("api/user/{Id}/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] GetProfileRequest request)
        {
            var maybeUser = await _mediator.Send(request);

            if (maybeUser.HasNoValue)
            {
                return UnprocessableEntity(Envelope.Failure("Profile does not exist"));
            }

            var user = maybeUser.Value;

            return Ok(Envelope.Success(user));
        }
    }
}
