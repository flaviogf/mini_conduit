using Conduit.Domain.Articles;

namespace Conduit.Application.Articles
{
    public sealed class ArticleResponse
    {
        public ArticleResponse(Article article)
        {
            Id = article.Id;
            Title = article.Title;
            Description = article.Description;
            Body = article.Body;
        }

        public string Id { get; }

        public string Title { get; }

        public string Description { get; }

        public string Body { get; }
    }
}
