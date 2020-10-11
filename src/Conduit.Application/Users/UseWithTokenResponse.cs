using Conduit.Domain.Users;

namespace Conduit.Application.Users
{
    public class UserWithTokenResponse : UserResponse
    {
        internal UserWithTokenResponse(User user, string token) : base(user)
        {
            Token = token;
        }

        public string Token { get; }
    }
}
