using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Api.Infrastructure
{
    public interface ITokenManager<T> where T : IdentityUser
    {
        Task<string> GenerateTokenAsync(T user);
    }
}
