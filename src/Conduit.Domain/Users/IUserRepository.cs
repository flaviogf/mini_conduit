using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Conduit.Domain.Users
{
    public interface IUserRepository
    {
        Task<Result> Add(User user);

        Task<Maybe<User>> FindByEmail(string email);

        Task<bool> CheckEmail(string email);
    }
}
