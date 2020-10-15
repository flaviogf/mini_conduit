using System.Threading.Tasks;
using Conduit.Application.Tags;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Presentation.Api.Tags
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/tag")]
    public class TagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetTagsRequest request)
        {
            var tags = await _mediator.Send(request);

            return Ok(Envelope.Success(tags));
        }
    }
}
