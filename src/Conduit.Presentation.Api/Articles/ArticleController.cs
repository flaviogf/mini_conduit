using System.Threading.Tasks;
using Conduit.Application.Articles;
using Conduit.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Presentation.Api.Articles
{
    [ApiController]
    [Authorize]
    [Route("api/article")]
    public class ArticleController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IUnitOfWork _uow;

        public ArticleController(IMediator mediator, IUnitOfWork uow)
        {
            _mediator = mediator;
            _uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArticleRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.IsFailure)
            {
                _uow.Rollback();

                return UnprocessableEntity(result.Error);
            }

            var article = result.Value;

            _uow.Commit();

            return Ok(article);
        }
    }
}
