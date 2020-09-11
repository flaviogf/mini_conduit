namespace Conduit.Core.Users
{
    public class User
    {
        public User(string username, string email, string bio = default, string image = default)
        {
            Username = username;
            Email = email;
            Bio = bio;
            Image = image;
        }

        public string Username { get; }

        public string Email { get; }

        public string Bio { get; }

        public string Image { get; }
    }
}
