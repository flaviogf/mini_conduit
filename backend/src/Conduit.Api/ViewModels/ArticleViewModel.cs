using System.Collections.Generic;

namespace Conduit.Api.ViewModels
{
    public class ArticleViewModel
    {
        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public IEnumerable<string> TagList { get; set; }
    }
}
