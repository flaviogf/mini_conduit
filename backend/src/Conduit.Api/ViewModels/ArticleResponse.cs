using Conduit.Api.Models;

namespace Conduit.Api.ViewModels
{
    public class ArticleResponse
    {
        public ArticleResponse(Article article)
        {
            Article = article;
        }

        public Article Article { get; }
    }
}
