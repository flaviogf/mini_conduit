using System.Collections.Generic;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Core
{
    public sealed class CreateArticleRequest : IRequest<Result>
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
