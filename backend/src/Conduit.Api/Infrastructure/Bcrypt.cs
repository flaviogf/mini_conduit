using System.Threading.Tasks;

namespace Conduit.Api.Infrastructure
{
    public class Bcrypt : IHash
    {
        public Task<string> Make(string value)
        {
            string result = BCrypt.Net.BCrypt.HashPassword(value);

            return Task.FromResult(result);
        }

        public Task<bool> Verify(string value, string hash)
        {
            bool result = BCrypt.Net.BCrypt.Verify(value, hash);

            return Task.FromResult(result);
        }
    }
}
