using System.Threading.Tasks;
using Conduit.Api.Models;

namespace Conduit.Api.Repositories
{
    public interface IArticleRepository
    {
        Task Save(Article article);
    }
}
