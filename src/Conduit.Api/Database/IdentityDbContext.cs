using Conduit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Database
{
    public class IdentityDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<User, Role, string>
    {
        public IdentityDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
