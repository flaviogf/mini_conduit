using System;

namespace Conduit.Api.Models
{
    public class UserArticle
    {
        public User User { get; set; }

        public string UserId { get; set; }

        public Article Article { get; set; }

        public string ArticleId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserArticle article && UserId == article.UserId && ArticleId == article.ArticleId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, ArticleId);
        }
    }
}
