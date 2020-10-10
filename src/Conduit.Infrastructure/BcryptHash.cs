using System.Threading.Tasks;
using Conduit.Application;

namespace Conduit.Infrastructure
{
    public class BcryptHash : IHash
    {
        public Task<string> Make(string value)
        {
            var result = BCrypt.Net.BCrypt.HashPassword(value);

            return Task.FromResult(result);
        }
    }
}
