using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Users
{
    public class FollowUserRequest : IRequest<Result<UserResponse>>
    {
        [Required]
        [StringLength(36, MinimumLength = 36)]
        public string Id { get; set; }
    }
}
