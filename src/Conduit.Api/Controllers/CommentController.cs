using System.Threading.Tasks;
using Conduit.Api.Database;
using Conduit.Api.Models;
using Conduit.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Res = Conduit.Api.Infrastructure.Response;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/article/{articleId}/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ConduitDbContext _context;

        private readonly UserManager<User> _userManager;

        public CommentController(ConduitDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] string articleId, [FromBody] NewCommentRequest req)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(it => it.Id == articleId);

            if (article == null)
            {
                return NotFound(Res.Failure("Article does not exist"));
            }

            var user = await _userManager.GetUserAsync(User);

            user.Comment(article, req.Text);

            await _context.SaveChangesAsync();

            return Ok(Res.Success(article.Id));
        }
    }
}
