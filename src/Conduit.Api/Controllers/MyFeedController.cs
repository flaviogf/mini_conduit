using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.Database;
using Conduit.Api.Infrastructure;
using Conduit.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Res = Conduit.Api.Infrastructure.Response;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/feed/mine")]
    public class MyFeedController : ControllerBase
    {
        private readonly ConduitDbContext _context;

        private readonly IMapper _mapper;

        public MyFeedController(ConduitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var articles = await _context.Articles
                .FromSqlInterpolated($"SELECT a.* FROM Articles a JOIN UserSubscription us ON a.AuthorId = us.SubscriptionId WHERE us.SubscriberId = {userId}")
                .Include(it => it.Author)
                .Include(it => it.Tags)
                .ThenInclude(it => it.Tag)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var total = await _context.Articles
                .FromSqlInterpolated($"SELECT a.* FROM Articles a JOIN UserSubscription us ON a.AuthorId = us.SubscriptionId WHERE us.SubscriberId = {userId}")
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
