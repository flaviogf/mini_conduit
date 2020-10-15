using System.Collections.Generic;
using MediatR;

namespace Conduit.Application.Tags
{
    public class GetTagsRequest : IRequest<IEnumerable<TagResponse>>
    {
    }
}
