using System;
using System.Collections.Generic;

namespace Conduit.Api.Models
{
    public class Article
    {
        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public IEnumerable<string> TagList { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Favorited { get; set; }

        public int FavoritesCount { get; set; }

        public User Author { get; set; }
    }
}
