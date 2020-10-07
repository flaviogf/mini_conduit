using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain.Articles;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Articles
{
    public sealed class CreateArticleHandler : IRequestHandler<CreateArticleRequest, Result<ArticleResponse>>
    {
        private readonly IAuth _auth;

        private readonly IArticleRepository _articleRepository;

        public CreateArticleHandler(IAuth auth, IArticleRepository articleRepository)
        {
            _auth = auth;
            _articleRepository = articleRepository;
        }

        public async Task<Result<ArticleResponse>> Handle(CreateArticleRequest request, CancellationToken cancellationToken)
        {
            var maybeUser = await _auth.GetUser();

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<ArticleResponse>("Current user wasn't found");
            }

            var user = maybeUser.Value;

            var article = user.WriteArticle(request.Id, request.Title, request.Description, request.Body);

            var result = await _articleRepository.Add(article);

            if (result.IsFailure)
            {
                return Result.Failure<ArticleResponse>(result.Error);
            }

            return Result.Success(new ArticleResponse(article));
        }
    }
}
