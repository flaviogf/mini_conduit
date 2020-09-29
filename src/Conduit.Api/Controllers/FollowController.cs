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
    [Route("api/user/{subscriptionId}/follow")]
    public class FollowController : ControllerBase
    {
        private readonly ConduitDbContext _context;

        public FollowController(ConduitDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] string subscriptionId)
        {
            var subscription = await _context.Users
                .FirstOrDefaultAsync(it => it.Id == subscriptionId);

            if (subscription == null)
            {
                return UnprocessableEntity(Res.Failure("The user you wanna subscribe does not exist."));
            }

            var subscriber = await _context.Users
                .Include(it => it.Subscriptions)
                .FirstOrDefaultAsync(it => it.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (subscriber == null)
            {
                return UnprocessableEntity(Res.Failure("The current user does not exist."));
            }

            if (subscriber.Equals(subscription))
            {
                return UnprocessableEntity(Res.Failure("You cannot subscribe to yourself."));
            }

            if (subscriber.HasSubscription(subscription))
            {
                return UnprocessableEntity(Res.Failure("You already are a subscriber of this user."));
            }

            subscriber.AddSubscription(subscription);

            await _context.SaveChangesAsync();

            return Ok(Res.Success());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] string subscriptionId)
        {
            var subscription = await _context.Users
                .FirstOrDefaultAsync(it => it.Id == subscriptionId);

            if (subscription == null)
            {
                return UnprocessableEntity(Res.Failure("The user you wanna unsubscribe does not exist."));
            }

            var subscriber = await _context.Users
                .Include(it => it.Subscriptions)
                .FirstOrDefaultAsync(it => it.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (subscriber == null)
            {
                return UnprocessableEntity(Res.Failure("The current user does not exist."));
            }

            if (subscriber.Equals(subscription))
            {
                return UnprocessableEntity(Res.Failure("You cannot unsubscribe to yourself."));
            }

            if (!subscriber.HasSubscription(subscription))
            {
                return UnprocessableEntity(Res.Failure("You are not a subscriber of this user."));
            }

            subscriber.RemoveSubscription(subscription);

            await _context.SaveChangesAsync();

            return Ok(Res.Success());
        }
    }
}
