using System.Collections.Generic;
using Conduit.Api.Models;

namespace Conduit.Api.ViewModels
{
    public class ArticlesResponse
    {
        public ArticlesResponse(IEnumerable<Article> articles)
        {
            Articles = articles;
        }

        public IEnumerable<Article> Articles { get; }
    }
}
