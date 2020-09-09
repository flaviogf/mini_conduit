using System.Threading.Tasks;

namespace Conduit.Core.Articles
{
    public interface IArticleRepository
    {
        Task Add(Article article);
    }
}
