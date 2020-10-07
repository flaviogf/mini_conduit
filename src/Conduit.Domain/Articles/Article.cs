using Conduit.Domain.Users;

namespace Conduit.Domain.Articles
{
    public class Article
    {
        public Article(string id, string title, string description, string body, User author)
        {
            Id = id;
            Title = title;
            Description = description;
            Body = body;
            Author = author;
        }

        public string Id { get; }

        public string Title { get; }

        public string Description { get; }

        public string Body { get; }

        public User Author { get; }
    }
}
