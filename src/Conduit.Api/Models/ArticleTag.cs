using System;

namespace Conduit.Api.Models
{
    public class ArticleTag
    {
        public Article Article { get; set; }

        public string ArticleId { get; set; }

        public Tag Tag { get; set; }

        public string TagId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ArticleTag tag && ArticleId == tag.ArticleId && TagId == tag.TagId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ArticleId, TagId);
        }
    }
}
