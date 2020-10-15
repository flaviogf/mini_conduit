using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain.Users;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Users
{
    public class GetProfileHandler : IRequestHandler<GetProfileRequest, Maybe<UserResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetProfileHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Maybe<UserResponse>> Handle(GetProfileRequest request, CancellationToken cancellationToken)
        {
            var maybeUser = await _userRepository.Find(request.Id);

            if (maybeUser.HasNoValue)
            {
                return null;
            }

            var user = maybeUser.Value;

            return new UserResponse(user);
        }
    }
}
