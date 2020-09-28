using System;
using System.Collections.Generic;

namespace Conduit.Api.Models
{
    public class Article
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public string AuthorId { get; set; }

        public IList<ArticleTag> Tags { get; set; } = new List<ArticleTag>();

        public void AddTags(IEnumerable<Tag> tags)
        {
            foreach (var tag in tags)
            {
                AddTag(tag);
            }
        }

        private void AddTag(Tag tag)
        {
            Tags.Add(new ArticleTag { Article = this, Tag = tag });
        }

        public override bool Equals(object obj)
        {
            return obj is Article article && Id == article.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
