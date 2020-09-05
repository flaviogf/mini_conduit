using Conduit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
        }
    }
}
