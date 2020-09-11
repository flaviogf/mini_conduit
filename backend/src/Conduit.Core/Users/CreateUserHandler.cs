using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Core.Users
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, Result<CreateUserResponse>>
    {
        private readonly IUserRepository _userRepository;

        private readonly ITokenService _tokenService;

        public CreateUserHandler(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<Result<CreateUserResponse>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _userRepository.FindByEmail(request.Email);

            if (maybeUser.HasValue)
            {
                return Result.Failure<CreateUserResponse>("This email is already taken");
            }

            var user = new User(request.Username, request.Email);

            await _userRepository.Add(user, request.Password);

            string token = await _tokenService.Generate(user);

            return Result.Success(new CreateUserResponse(user, token));
        }
    }
}
