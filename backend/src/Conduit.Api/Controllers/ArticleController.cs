using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.ViewModels;
using Conduit.Core.Articles;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public class ArticleController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ArticleController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArticleRequestViewModel viewModel)
        {
            CreateArticleRequest request = _mapper.Map<CreateArticleRequest>(viewModel.Article);

            Result<CreateArticleResponse> result = await _mediator.Send(request);

            if (result.IsFailure)
            {
                return StatusCode(402, new ErrorResponseViewModel(result.Error));
            }

            ArticleViewModel article = _mapper.Map<ArticleViewModel>(result.Value);

            return StatusCode(201, new ArticleResponseViewModel(article));
        }
    }
}
