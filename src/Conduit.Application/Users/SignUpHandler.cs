using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain.Users;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Users
{
    public class SignUpHandler : IRequestHandler<SignUpRequest, Result<UserResponse>>
    {
        private readonly IUserRepository _userRepository;

        private readonly IHash _hash;

        public SignUpHandler(IUserRepository userRepository, IHash hash)
        {
            _userRepository = userRepository;
            _hash = hash;
        }

        public async Task<Result<UserResponse>> Handle(SignUpRequest request, CancellationToken cancellationToken)
        {
            if (await _userRepository.CheckEmail(request.Email))
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

            var result = await _userRepository.Insert(user);

            if (result.IsFailure)
            {
                return Result.Failure<UserResponse>(result.Error);
            }

            return Result.Success(new UserResponse(user));
        }
    }
}
