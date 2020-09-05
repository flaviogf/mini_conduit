using System.Threading.Tasks;
using Conduit.Api.Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Repositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public EFUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Save(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<Maybe<User>> Find(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(it => it.Id == id);
        }

        public async Task<Maybe<User>> FindByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(it => it.Email == email);
        }
    }
}
