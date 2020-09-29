using Conduit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Database
{
    public class IdentityDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<User, Role, string>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<UserSubscription>()
                .HasKey(it => new { it.SubscriberId, it.SubscriptionId });

            builder
                .Entity<UserSubscription>()
                .HasOne(it => it.Subscriber)
                .WithMany(it => it.Subscriptions);

            builder
                .Entity<UserSubscription>()
                .HasOne(it => it.Subscription)
                .WithMany(it => it.Subscribers);
        }
    }
}
