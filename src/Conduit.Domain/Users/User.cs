using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Conduit.Domain.Users
{
    public class User
    {
        private IList<User> _followers = new List<User>();

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

        public IReadOnlyList<User> Followers => new ReadOnlyCollection<User>(_followers);

        public void Follow(User user)
        {
            user.AddFollower(this);
        }

        public void AddFollower(IEnumerable<User> followers)
        {
            foreach (var it in followers)
            {
                _followers.Add(it);
            }
        }

        public void AddFollower(params User[] followers)
        {
            foreach (var it in followers)
            {
                _followers.Add(it);
            }
        }
    }
}
