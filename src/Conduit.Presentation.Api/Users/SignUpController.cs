using System.Threading.Tasks;
using Conduit.Application.Users;
using Conduit.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Presentation.Api.Users
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/sign-up")]
    public class SignUpController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IUnitOfWork _uow;

        public SignUpController(IMediator mediator, IUnitOfWork uow)
        {
            _mediator = mediator;
            _uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SignUpRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.IsFailure)
            {
                _uow.Rollback();

                return UnprocessableEntity(result.Error);
            }

            var user = result.Value;

            _uow.Commit();

            return Ok(user);
        }
    }
}
