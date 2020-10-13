using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain.Users;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Users
{
    public class FollowUserHandler : IRequestHandler<FollowUserRequest, Result<UserResponse>>
    {
        private readonly IUserRepository _userRepository;

        private readonly IAuth _auth;

        public FollowUserHandler(IUserRepository userRepository, IAuth auth)
        {
            _userRepository = userRepository;
            _auth = auth;
        }

        public async Task<Result<UserResponse>> Handle(FollowUserRequest request, CancellationToken cancellationToken)
        {
            var maybeFollower = await _auth.GetUser();

            if (maybeFollower.HasNoValue)
            {
                return Result.Failure<UserResponse>("Current user was not found");
            }

            var maybeUser = await _userRepository.Find(request.Id);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<UserResponse>("User does not exist");
            }

            var follower = maybeFollower.Value;

            var user = maybeUser.Value;

            follower.Follow(user);

            var result = await _userRepository.Update(user);

            if (result.IsFailure)
            {
                return Result.Failure<UserResponse>(result.Error);
            }

            return Result.Success(new UserResponse(user));
        }
    }
}
