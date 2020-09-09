using System.Threading.Tasks;

namespace Conduit.Core
{
    public interface IArticleRepository
    {
        Task Add(Article article);
    }
}
