using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Conduit.Core.Articles
{
    public class Article
    {
        private readonly List<Tag> _tags;

        public Article(string title, string description, string body)
        {
            Slug = new Slug(title);
            Title = title;
            Description = description;
            Body = body;
            _tags = new List<Tag>();
        }

        public Slug Slug { get; }

        public string Title { get; }

        public string Description { get; }

        public string Body { get; }

        public IReadOnlyList<Tag> Tags => new ReadOnlyCollection<Tag>(_tags);

        public void AddTags(IEnumerable<string> tagList)
        {
            IEnumerable<Tag> tags = tagList.Select(it => new Tag(it));

            _tags.AddRange(tags);
        }
    }
}
