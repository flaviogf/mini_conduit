using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Api.Database;
using Conduit.Api.Infrastructure;
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
    [Route("api/article")]
    public class ArticleController : ControllerBase
    {
        private readonly ConduitDbContext _context;

        private readonly UserManager<User> _userManager;

        public ArticleController(ConduitDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewArticleRequest req)
        {
            var user = await _userManager.GetUserAsync(User);

            var article = new Article
            {
                Id = Guid.NewGuid().ToString(),
                Title = req.Title,
                Description = req.Description,
                Body = req.Body,
                AuthorId = user.Id
            };

            var tags = await GetOrCreateTags(req.Tags);

            article.AddTags(tags);

            await _context.Articles.AddAsync(article);

            await _context.SaveChangesAsync();

            return Created($"api/article/{article.Id}", Res.Success(article.Id));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var pagination = Pagination.Of(_context.Articles, page, pageSize);

            return Ok(Res.Success(pagination));
        }

        private async Task<IEnumerable<Tag>> GetOrCreateTags(IEnumerable<string> tags)
        {
            var tasks = tags.Select(GetOrCreateTag);

            return await Task.WhenAll(tasks);
        }

        private async Task<Tag> GetOrCreateTag(string tagName)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(it => it.Name == tagName);

            return tag ?? new Tag { Id = Guid.NewGuid().ToString(), Name = tagName };
        }
    }
}
