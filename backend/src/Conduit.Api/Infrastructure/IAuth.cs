using System.Threading.Tasks;
using Conduit.Api.Models;
using CSharpFunctionalExtensions;

namespace Conduit.Api.Infrastructure
{
    public interface IAuth
    {
        Task<Result<User>> Attempt(string email, string password);
    }
}
