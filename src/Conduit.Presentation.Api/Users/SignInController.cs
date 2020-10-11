using System.Threading.Tasks;
using Conduit.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Presentation.Api.Users
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/sign-in")]
    public class SignInController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SignInController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SignInRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.IsFailure)
            {
                return UnprocessableEntity(Envelope.Failure(result.Error));
            }

            var user = result.Value;

            return Ok(Envelope.Success(user));
        }
    }
}
