using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Core.Articles
{
    public class CreateArticleHandler : IRequestHandler<CreateArticleRequest, Result<CreateArticleResponse>>
    {
        private readonly IArticleRepository _articleRepository;

        public CreateArticleHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<Result<CreateArticleResponse>> Handle(CreateArticleRequest request, CancellationToken cancellationToken)
        {
            var article = new Article(request.Title, request.Description, request.Body);

            article.AddTags(request.TagList);

            await _articleRepository.Add(article);

            return Result.Success(new CreateArticleResponse(article));
        }
    }
}
