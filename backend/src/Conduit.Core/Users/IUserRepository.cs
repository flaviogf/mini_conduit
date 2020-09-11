using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Conduit.Core.Users
{
    public interface IUserRepository
    {
        Task Add(User user, string password);

        Task<Maybe<User>> FindByEmail(string email);
    }
}
