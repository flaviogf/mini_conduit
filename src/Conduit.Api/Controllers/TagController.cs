using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Api.Database;
using Conduit.Api.ViewModels;
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

        private readonly IMapper _mapper;

        public TagController(ConduitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tags = await _context.Tags
                .AsNoTracking()
                .ToListAsync();

            return Ok(Res.Success(_mapper.Map<IEnumerable<TagViewModel>>(tags)));
        }
    }
}
