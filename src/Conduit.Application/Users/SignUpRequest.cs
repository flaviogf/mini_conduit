using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Users
{
    public sealed class SignUpRequest : IRequest<Result<UserResponse>>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
