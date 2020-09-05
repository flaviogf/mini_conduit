using System.Threading.Tasks;

namespace Conduit.Api.Infrastructure
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
