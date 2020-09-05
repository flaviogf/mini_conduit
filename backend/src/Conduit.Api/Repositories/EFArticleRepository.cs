using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Article> Filter(int offset, int limit)
        {
            return _context.Articles.Include(it => it.Author).OrderByDescending(it => it.CreatedAt).Skip(offset).Take(limit);
        }
    }
}
