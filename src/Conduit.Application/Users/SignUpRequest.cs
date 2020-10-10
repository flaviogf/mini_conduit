using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Users
{
    public class SignUpRequest : IRequest<Result<UserResponse>>
    {
        [Required]
        [StringLength(36, MinimumLength = 36)]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
