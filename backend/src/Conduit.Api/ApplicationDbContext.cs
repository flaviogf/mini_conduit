using System.Collections.Generic;
using Conduit.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Conduit.Api
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region User

            modelBuilder
                .Entity<User>()
                .HasKey(it => it.Id);

            modelBuilder
                .Entity<User>()
                .Property(it => it.Username)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .Property(it => it.Email)
                .IsRequired();

            modelBuilder
                   .Entity<User>()
                   .HasIndex(it => it.Email)
                   .IsUnique();

            modelBuilder
                .Entity<User>()
                .Property(it => it.PasswordHash)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .Ignore(it => it.Token);

            modelBuilder
                .Entity<User>()
                .Property(it => it.Bio);

            modelBuilder
                .Entity<User>()
                .Property(it => it.Image);

            #endregion

            #region Article

            modelBuilder
                .Entity<Article>()
                .HasKey(it => it.Slug);

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
                .Property(it => it.TagList)
                .HasConversion(it => JsonConvert.SerializeObject(it), it => JsonConvert.DeserializeObject<IEnumerable<string>>(it));

            modelBuilder
                .Entity<Article>()
                .Property(it => it.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("date('now')");

            modelBuilder
                .Entity<Article>()
                .Property(it => it.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("date('now')");

            modelBuilder
                .Entity<Article>()
                .Ignore(it => it.Favorited);

            modelBuilder
                .Entity<Article>()
                .Ignore(it => it.FavoritesCount);

            modelBuilder
                .Entity<Article>()
                .HasOne(it => it.Author);

            #endregion
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            ILoggerFactory factory = LoggerFactory.Create(it => it.AddConsole());

            optionsBuilder.UseLoggerFactory(factory);
        }
    }
}
