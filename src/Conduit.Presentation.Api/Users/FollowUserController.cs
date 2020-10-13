using System.Threading.Tasks;
using Conduit.Application.Users;
using Conduit.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Presentation.Api.Users
{
    [ApiController]
    [Authorize]
    [Route("api/user/{Id}/follow")]
    public class FollowUserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        private readonly IMediator _mediator;

        public FollowUserController(IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] FollowUserRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.IsFailure)
            {
                _uow.Rollback();

                return UnprocessableEntity(Envelope.Failure(result.Error));
            }

            var user = result.Value;

            _uow.Commit();

            return Ok(Envelope.Success(user));
        }
    }
}
