using System.Threading.Tasks;

namespace Conduit.Application
{
    public interface IHash
    {
        Task<bool> Compare(string value, string hash);

        Task<string> Make(string value);
    }
}
