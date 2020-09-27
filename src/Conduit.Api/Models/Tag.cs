using System.Collections.Generic;

namespace Conduit.Api.Models
{
    public class Tag
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<ArticleTag> Articles { get; set; } = new List<ArticleTag>();
    }
}
