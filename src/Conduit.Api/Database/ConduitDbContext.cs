using Conduit.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Database
{
    public class ConduitDbContext : IdentityDbContext<User>
    {
        public ConduitDbContext(DbContextOptions<ConduitDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Article

            builder
                .Entity<Article>()
                .HasKey(it => it.Id);

            builder
                .Entity<Article>()
                .Property(it => it.Title)
                .IsRequired();

            builder
                .Entity<Article>()
                .Property(it => it.Description)
                .IsRequired();

            builder
                .Entity<Article>()
                .Property(it => it.Body)
                .IsRequired();

            builder
                .Entity<Article>()
                .HasOne(it => it.Author)
                .WithMany(it => it.Articles);

            builder
                .Entity<Article>()
                .HasMany(it => it.Tags)
                .WithOne(it => it.Article);

            #endregion

            #region ArticleTag

            builder
                .Entity<ArticleTag>()
                .HasKey(it => new { it.ArticleId, it.TagId });

            builder
                .Entity<ArticleTag>()
                .HasOne(it => it.Article)
                .WithMany(it => it.Tags);

            builder
                .Entity<ArticleTag>()
                .Property(it => it.ArticleId)
                .IsRequired();

            builder
                .Entity<ArticleTag>()
                .HasOne(it => it.Tag)
                .WithMany(it => it.Articles);

            builder
                .Entity<ArticleTag>()
                .Property(it => it.TagId)
                .IsRequired();

            #endregion

            #region Tag

            builder
                .Entity<Tag>()
                .HasKey(it => it.Id);

            builder
                .Entity<Tag>()
                .Property(it => it.Name)
                .IsRequired();

            builder
                .Entity<Tag>()
                .HasMany(it => it.Articles)
                .WithOne(it => it.Tag);

            #endregion

            #region User

            builder
                .Entity<User>()
                .Property(it => it.Avatar);

            builder
                .Entity<User>()
                .Property(it => it.Bio);

            #endregion

            #region UserSubscription

            builder
                .Entity<UserSubscription>()
                .HasKey(it => new { it.SubscriberId, it.SubscriptionId });

            builder
                .Entity<UserSubscription>()
                .HasOne(it => it.Subscriber)
                .WithMany(it => it.Subscriptions);

            builder
                .Entity<UserSubscription>()
                .Property(it => it.SubscriberId)
                .IsRequired();

            builder
                .Entity<UserSubscription>()
                .HasOne(it => it.Subscription)
                .WithMany(it => it.Subscribers);

            #endregion
        }
    }
}
