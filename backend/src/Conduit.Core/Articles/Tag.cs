namespace Conduit.Core.Articles
{
    public class Tag
    {
        public Tag(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator string(Tag tag)
        {
            return tag.Name;
        }
    }
}
