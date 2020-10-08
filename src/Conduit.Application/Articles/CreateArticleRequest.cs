using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Articles
{
    public sealed class CreateArticleRequest : IRequest<Result<ArticleResponse>>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }
    }
}
