using System.Collections.Generic;
using System.Linq;

namespace Conduit.Core.Articles
{
    public class CreateArticleResponse
    {
        private readonly Article _article;

        internal CreateArticleResponse(Article article)
        {
            _article = article;
        }

        public string Slug => _article.Slug;

        public string Title => _article.Title;

        public string Description => _article.Description;

        public string Body => _article.Body;

        public IEnumerable<string> TagList => _article.Tags.Select(it => it.ToString());
    }
}
