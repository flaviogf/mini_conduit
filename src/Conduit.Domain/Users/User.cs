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
    }
}
