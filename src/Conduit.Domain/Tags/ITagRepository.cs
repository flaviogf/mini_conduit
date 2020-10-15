using System.Collections.Generic;
using System.Threading.Tasks;

namespace Conduit.Domain.Tags
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> Find();
    }
}
