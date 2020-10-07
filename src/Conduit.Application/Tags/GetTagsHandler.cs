using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain.Tags;
using CSharpFunctionalExtensions;
using MediatR;

namespace Conduit.Application.Tags
{
    public sealed class GetTagsHandler : IRequestHandler<GetTagsRequest, IEnumerable<TagResponse>>
    {
        private readonly ITagRepository _tagRepository;

        public GetTagsHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<TagResponse>> Handle(GetTagsRequest request, CancellationToken cancellationToken)
        {
            var tags = await _tagRepository.FindAll();

            return tags.Select(it => new TagResponse(it));
        }
    }
}
