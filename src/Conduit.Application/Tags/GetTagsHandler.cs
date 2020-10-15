using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain.Tags;
using MediatR;

namespace Conduit.Application.Tags
{
    public class GetTagsHandler : IRequestHandler<GetTagsRequest, IEnumerable<TagResponse>>
    {
        private readonly ITagRepository _tagRepository;

        public GetTagsHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<TagResponse>> Handle(GetTagsRequest request, CancellationToken cancellationToken)
        {
            var tags = await _tagRepository.Find();

            return tags.Select(it => new TagResponse(it));
        }
    }
}
