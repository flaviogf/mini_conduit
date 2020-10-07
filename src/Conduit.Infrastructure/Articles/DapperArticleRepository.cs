using System.Threading.Tasks;
using Conduit.Domain.Articles;
using CSharpFunctionalExtensions;

namespace Conduit.Infrastructure.Articles
{
    public class DapperArticleRepository : IArticleRepository
    {
        public Task<Result> Add(Article article)
        {
            throw new System.NotImplementedException();
        }
    }
}
