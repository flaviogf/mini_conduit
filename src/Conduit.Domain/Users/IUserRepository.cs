using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Conduit.Domain.Users
{
    public interface IUserRepository
    {
        Task<Result> Insert(User user);

        Task<Result> Update(User user);

        Task<Maybe<User>> Find(string id);

        Task<Maybe<User>> FindByEmail(string email);

        Task<bool> CheckEmail(string email);
    }
}
