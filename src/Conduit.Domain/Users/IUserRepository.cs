using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Conduit.Domain.Users
{
    public interface IUserRepository
    {
        Task<Result> Add(User user);

        Task<bool> CheckEmail(string email);
    }
}
