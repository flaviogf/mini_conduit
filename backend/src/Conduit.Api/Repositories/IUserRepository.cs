using System.Threading.Tasks;
using Conduit.Api.Models;
using CSharpFunctionalExtensions;

namespace Conduit.Api.Repositories
{
    public interface IUserRepository
    {
        Task Save(User user);

        Task<Maybe<User>> Find(string id);

        Task<Maybe<User>> FindByEmail(string email);
    }
}
