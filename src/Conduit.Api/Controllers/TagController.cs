using System.Threading.Tasks;
using Conduit.Api.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Res = Conduit.Api.Infrastructure.Response;

namespace Conduit.Api.Controllers
{
    [ApiController]
    [Route("api/tag")]
    public class TagController : ControllerBase
    {
        private readonly ConduitDbContext _context;

        public TagController(ConduitDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tags = await _context.Tags
                .AsNoTracking()
                .ToListAsync();

            return Ok(Res.Success(tags));
        }
    }
}
