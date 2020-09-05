using System.Collections.Generic;
using System.Threading.Tasks;
using Conduit.Api.Models;
using CSharpFunctionalExtensions;

namespace Conduit.Api.Repositories
{
    public interface IArticleRepository
    {
        Task Save(Article article);

        Task<Maybe<Article>> Find(string slug);

        IEnumerable<Article> Filter(int offset, int limit);
    }
}
