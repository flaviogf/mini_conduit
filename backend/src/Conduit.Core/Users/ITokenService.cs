using System.Threading.Tasks;

namespace Conduit.Core.Users
{
    public interface ITokenService
    {
        Task<string> Generate(User user);
    }
}
