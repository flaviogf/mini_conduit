using System.Threading.Tasks;
using Conduit.Application;
using Conduit.Domain.Users;
using CSharpFunctionalExtensions;

namespace Conduit.Infrastructure
{
    public class LocalAuth : IAuth
    {
        public Task<Maybe<User>> GetUser()
        {
            throw new System.NotImplementedException();
        }
    }
}
