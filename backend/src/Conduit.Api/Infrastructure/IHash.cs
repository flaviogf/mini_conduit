using System.Threading.Tasks;

namespace Conduit.Api.Infrastructure
{
    public interface IHash
    {
        Task<string> Make(string value);

        Task<bool> Verify(string value, string hash);
    }
}
