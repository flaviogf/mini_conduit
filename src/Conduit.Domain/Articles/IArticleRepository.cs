using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Conduit.Domain.Articles
{
    public interface IArticleRepository
    {
        Task<Result> Add(Article article);
    }
}
