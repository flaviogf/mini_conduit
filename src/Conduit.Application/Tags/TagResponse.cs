using Conduit.Domain.Tags;

namespace Conduit.Application.Tags
{
    public class TagResponse
    {
        internal TagResponse(Tag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
        }

        public string Id { get; }

        public string Name { get; }
    }
}
