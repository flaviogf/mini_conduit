using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Articles
{
    public sealed class CreateArticleRequest : IRequest<Result<ArticleResponse>>
    {
        public CreateArticleRequest(string id, string title, string description, string body)
        {
            Id = id;
            Title = title;
            Description = description;
            Body = body;
        }

        public string Id { get; }

        public string Title { get; }

        public string Description { get; }

        public string Body { get; }
    }
}
