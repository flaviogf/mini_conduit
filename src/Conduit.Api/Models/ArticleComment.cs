using System;

namespace Conduit.Api.Models
{
    public class ArticleComment
    {
        public string Id { get; set; }

        public Article Article { get; set; }

        public User User { get; set; }

        public string Text { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ArticleComment comment && Id == comment.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
