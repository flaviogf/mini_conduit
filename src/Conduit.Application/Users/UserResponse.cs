using Conduit.Domain.Users;

namespace Conduit.Application.Users
{
    public class UserResponse
    {
        internal UserResponse(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Bio = user.Bio;
            Avatar = user.Avatar;
        }

        public string Id { get; }

        public string UserName { get; }

        public string Email { get; }

        public string Bio { get; }

        public string Avatar { get; }
    }
}
