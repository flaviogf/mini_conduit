using Conduit.Api.Models;

namespace Conduit.Api.ViewModels
{
    public class NewUserResponse
    {
        public NewUserResponse(User user)
        {
            User = user;
        }

        public User User { get; }
    }
}
