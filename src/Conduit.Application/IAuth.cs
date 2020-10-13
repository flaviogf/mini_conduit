using System.Threading.Tasks;
using Conduit.Domain.Users;
using CSharpFunctionalExtensions;

namespace Conduit.Application
{
    public interface IAuth
    {
        Task<Maybe<User>> GetUser();
    }
}
