using System.Threading.Tasks;
using Conduit.Domain.Users;

namespace Conduit.Application
{
    public interface IToken
    {
        Task<string> Make(User user);
    }
}
