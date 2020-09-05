using System.Threading.Tasks;
using Conduit.Api.Models;

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
    }
}
