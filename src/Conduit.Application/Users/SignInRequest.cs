using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Users
{
    public class SignInRequest : IRequest<Result<UserWithTokenResponse>>
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
