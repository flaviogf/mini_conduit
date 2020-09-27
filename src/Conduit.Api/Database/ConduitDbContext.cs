using Conduit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Database
{
    public class ConduitDbContext : DbContext
    {
        public ConduitDbContext(DbContextOptions<ConduitDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Article>()
                .HasKey(it => it.Id);

            modelBuilder
                .Entity<Article>()
                .Property(it => it.Title)
                .IsRequired();

            modelBuilder
                .Entity<Article>()
                .Property(it => it.Description)
                .IsRequired();

            modelBuilder
                .Entity<Article>()
                .Property(it => it.Body)
                .IsRequired();

            modelBuilder
                .Entity<Article>()
                .Property(it => it.AuthorId)
                .IsRequired();

            modelBuilder
                .Entity<Article>()
                .HasMany(it => it.Tags)
                .WithOne(it => it.Article);

            modelBuilder
                .Entity<ArticleTag>()
                .HasKey(it => new { it.ArticleId, it.TagId });

            modelBuilder
                .Entity<Tag>()
                .HasKey(it => it.Id);

            modelBuilder
                .Entity<Tag>()
                .Property(it => it.Name)
                .IsRequired();

            modelBuilder
                .Entity<Tag>()
                .HasMany(it => it.Articles)
                .WithOne(it => it.Tag);
        }
    }
}
