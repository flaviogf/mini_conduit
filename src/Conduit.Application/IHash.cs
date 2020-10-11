using System.Threading.Tasks;

namespace Conduit.Application
{
    public interface IHash
    {
        Task<string> Make(string value);

        Task<bool> Compare(string value, string hash);
    }
}
