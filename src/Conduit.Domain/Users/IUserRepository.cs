using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Conduit.Domain.Users
{
    public interface IUserRepository
    {
        Task<Result> Add(User user);

        Task<Maybe<User>> FindOne(string id);

        Task<bool> CheckId(string id);

        Task<bool> CheckUserName(string userName);

        Task<bool> CheckEmail(string email);
    }
}
