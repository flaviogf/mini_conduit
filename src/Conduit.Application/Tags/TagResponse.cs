using Conduit.Domain.Tags;

namespace Conduit.Application.Tags
{
    public sealed class TagResponse
    {
        internal TagResponse(Tag tag)
        {
            Name = tag.Name;
        }

        public string Name { get; }
    }
}
