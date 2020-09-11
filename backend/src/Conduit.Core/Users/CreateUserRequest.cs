using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Core.Users
{
    public sealed class CreateUserRequest : IRequest<Result<CreateUserResponse>>
    {
        public CreateUserRequest(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }

        public string Username { get; }

        public string Email { get; }

        public string Password { get; }
    }
}
