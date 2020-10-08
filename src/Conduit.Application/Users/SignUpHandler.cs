using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain.Users;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Users
{
    public sealed class SignUpHandler : IRequestHandler<SignUpRequest, Result<UserResponse>>
    {
        private readonly IHash _hash;

        private readonly IUserRepository _userRepository;

        public SignUpHandler(IHash hash, IUserRepository userRepository)
        {
            _hash = hash;
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponse>> Handle(SignUpRequest request, CancellationToken cancellationToken)
        {
            if ((await _userRepository.CheckId(request.Id)))
            {
                return Result.Failure<UserResponse>("This id is already taken");
            }

            if ((await _userRepository.CheckUserName(request.UserName)))
            {
                return Result.Failure<UserResponse>("This userName is already taken");
            }

            if ((await _userRepository.CheckEmail(request.Email)))
            {
                return Result.Failure<UserResponse>("This email is already taken");
            }

            var passwordHash = await _hash.Make(request.Password);

            var user = new User(
                id: request.Id,
                userName: request.UserName,
                email: request.Email,
                bio: string.Empty,
                avatar: string.Empty,
                passwordHash: passwordHash
            );

            var result = await _userRepository.Add(user);

            if (result.IsFailure)
            {
                return Result.Failure<UserResponse>(result.Error);
            }

            return Result.Success(new UserResponse(user));
        }
    }
}
