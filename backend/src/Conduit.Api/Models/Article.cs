using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Conduit.Api.Models
{
    [DataContract]
    public class Article
    {
        [DataMember]
        public string Slug { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public IEnumerable<string> TagList { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public DateTime UpdatedAt { get; set; }

        [DataMember]
        public bool Favorited { get; set; }

        [DataMember]
        public int FavoritesCount { get; set; }

        [DataMember]
        public User Author { get; set; }
    }
}
