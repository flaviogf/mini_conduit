using System.Collections.Generic;
using System.Threading.Tasks;
using Conduit.Api.Models;
using Conduit.Api.ViewModels;
using CSharpFunctionalExtensions;

namespace Conduit.Api.Repositories
{
    public interface IArticleRepository
    {
        Task Save(Article article);

        Task<Maybe<Article>> Find(string slug);

        IEnumerable<Article> Filter(ArticlesFilter filter, int offset, int limit);
    }
}
