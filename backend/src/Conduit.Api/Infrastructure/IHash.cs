using System.Threading.Tasks;

namespace Conduit.Api.Infrastructure
{
    public interface IHash
    {
        Task<string> Make(string value);
    }
}
