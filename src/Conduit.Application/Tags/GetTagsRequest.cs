using System.Collections.Generic;
using MediatR;

namespace Conduit.Application.Tags
{
    public sealed class GetTagsRequest : IRequest<IEnumerable<TagResponse>>
    {
    }
}
