using System.Security.Claims;
using System.Threading.Tasks;
using Conduit.Api.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Res = Conduit.Api.Infrastructure.Response;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/article/{articleId}/favorite")]
    public class FavoriteArticleController : ControllerBase
    {
        private readonly ConduitDbContext _context;

        public FavoriteArticleController(ConduitDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] string articleId)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(it => it.Id == articleId);

            if (article == null)
            {
                return NotFound(Res.Failure("Article does not exist."));
            }

            var user = await _context.Users
                .Include(it => it.Favorites)
                .FirstOrDefaultAsync(it => it.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user == null)
            {
                return UnprocessableEntity(Res.Failure("The current user does not exist"));
            }

            if (user.HasFavorite(article))
            {
                user.RemoveFavorite(article);

                await _context.SaveChangesAsync();

                return Ok(Res.Success(article.Id));
            }

            user.AddFavorite(article);

            await _context.SaveChangesAsync();

            return Ok(Res.Success(article.Id));
        }
    }
}
