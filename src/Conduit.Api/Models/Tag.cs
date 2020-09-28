using System;
using System.Collections.Generic;

namespace Conduit.Api.Models
{
    public class Tag
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<ArticleTag> Articles { get; set; } = new List<ArticleTag>();

        public override bool Equals(object obj)
        {
            return obj is Tag tag && Id == tag.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
