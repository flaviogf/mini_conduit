using System.Collections.Generic;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Core.Articles
{
    public sealed class CreateArticleRequest : IRequest<Result<CreateArticleResponse>>
    {
        public CreateArticleRequest(string title, string description, string body, IEnumerable<string> tagList)
        {
            Title = title;
            Description = description;
            Body = body;
            TagList = tagList;
        }

        public string Title { get; }

        public string Description { get; }

        public string Body { get; }

        public IEnumerable<string> TagList { get; }
    }
}
