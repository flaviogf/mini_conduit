using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain.Users;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Users
{
    public class SignInHandler : IRequestHandler<SignInRequest, Result<UserWithTokenResponse>>
    {
        private readonly IUserRepository _userRepository;

        private readonly IHash _hash;

        private readonly IToken _token;

        public SignInHandler(IUserRepository userRepository, IHash hash, IToken token)
        {
            _userRepository = userRepository;
            _hash = hash;
            _token = token;
        }

        public async Task<Result<UserWithTokenResponse>> Handle(SignInRequest request, CancellationToken cancellationToken)
        {
            var maybeUser = await _userRepository.FindByEmail(request.Email);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<UserWithTokenResponse>("Invalid email or password");
            }

            var user = maybeUser.Value;

            if (!(await _hash.Compare(request.Password, user.PasswordHash)))
            {
                return Result.Failure<UserWithTokenResponse>("Invalid email or password");
            }

            var token = await _token.Make(user);

            return Result.Success(new UserWithTokenResponse(user, token));
        }
    }
}
