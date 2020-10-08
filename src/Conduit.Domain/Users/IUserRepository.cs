using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Conduit.Domain.Users
{
    public interface IUserRepository
    {
        Task<Maybe<User>> FindOne(string id);
    }
}
