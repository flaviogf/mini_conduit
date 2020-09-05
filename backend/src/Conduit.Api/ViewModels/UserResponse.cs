using Conduit.Api.Models;

namespace Conduit.Api.ViewModels
{
    public class UserResponse
    {
        public UserResponse(User user)
        {
            User = user;
        }

        public User User { get; }
    }
}
