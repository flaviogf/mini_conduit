namespace Conduit.Core.Users
{
    public class CreateUserResponse
    {
        private readonly User _user;

        internal CreateUserResponse(User user, string token)
        {
            _user = user;
            Token = token;
        }

        public string Username => _user.Username;

        public string Email => _user.Email;

        public string Token { get; }

        public string Bio => _user.Bio;

        public string Image => _user.Image;
    }
}
