using Conduit.Domain.Articles;

namespace Conduit.Domain.Users
{
    public class User
    {
        public User(string id, string userName, string email, string bio, string avatar, string passwordHash)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Bio = bio;
            Avatar = avatar;
            PasswordHash = passwordHash;
        }

        public string Id { get; }

        public string UserName { get; }

        public string Email { get; }

        public string Bio { get; }

        public string Avatar { get; }

        public string PasswordHash { get; }

        public Article WriteArticle(string id, string title, string description, string body)
        {
            return new Article(id, title, description, body, this);
        }
    }
}
