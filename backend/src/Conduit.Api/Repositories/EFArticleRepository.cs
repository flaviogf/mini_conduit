using System.Threading.Tasks;
using Conduit.Api.Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Repositories
{
    public class EFArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public EFArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Save(Article article)
        {
            await _context.Articles.AddAsync(article);
        }

        public async Task<Maybe<Article>> Find(string slug)
        {
            return await _context.Articles.Include(it => it.Author).FirstOrDefaultAsync(it => it.Slug == slug);
        }
    }
}
