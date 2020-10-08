using System.Threading.Tasks;
using Conduit.Application;

namespace Conduit.Infrastructure
{
    public class BCryptHash : IHash
    {
        public Task<bool> Compare(string value, string hash)
        {
            var result = BCrypt.Net.BCrypt.Verify(value, hash);

            return Task.FromResult(result);
        }

        public Task<string> Make(string value)
        {
            var result = BCrypt.Net.BCrypt.HashPassword(value);

            return Task.FromResult(result);
        }
    }
}
