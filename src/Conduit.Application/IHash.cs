using System.Threading.Tasks;

namespace Conduit.Application
{
    public interface IHash
    {
        Task<string> Make(string value);
    }
}
