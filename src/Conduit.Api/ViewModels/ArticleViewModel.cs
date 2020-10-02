using System.Collections.Generic;

namespace Conduit.Api.ViewModels
{
    public class ArticleViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public UserViewModel Author { get; set; }

        public IList<ArticleTagViewModel> Tags { get; set; }

        public IList<ArticleCommentViewModel> Comments { get; set; }
    }
}
