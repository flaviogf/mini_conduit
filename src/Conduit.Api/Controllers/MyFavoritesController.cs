using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    [Route("api/favorite/mine")]
    public class MyFavoritesController : ControllerBase
    {
        private readonly ConduitDbContext _context;

        private readonly UserManager<User> _userManager;

        private readonly IMapper _mapper;

        public MyFavoritesController(ConduitDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var user = await _userManager.GetUserAsync(User);

            var articles = await _context.Articles
                .FromSqlInterpolated($"SELECT a.* FROM Articles a JOIN UserArticle ua ON a.Id = ua.ArticleId WHERE ua.UserId = {user.Id}")
                .Include(it => it.Author)
                .Include(it => it.Tags)
                .ThenInclude(it => it.Tag)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var total = await _context.Articles
                .FromSqlInterpolated($"SELECT a.* FROM Articles a JOIN UserArticle ua ON a.Id = ua.ArticleId WHERE ua.UserId = {user.Id}")
                .Include(it => it.Author)
                .CountAsync();

            var pagination = Pagination.Of
            (
                items: _mapper.Map<IEnumerable<ArticleViewModel>>(articles),
                page: page,
                pageSize: pageSize,
                total: total
            );

            return Ok(Res.Success(pagination));
        }
    }
}
