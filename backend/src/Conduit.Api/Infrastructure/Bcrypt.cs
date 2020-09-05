using System.Threading.Tasks;

namespace Conduit.Api.Infrastructure
{
    public class Bcrypt : IHash
    {
        public Task<string> Make(string value)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(value);

            return Task.FromResult(hash);
        }
    }
}
