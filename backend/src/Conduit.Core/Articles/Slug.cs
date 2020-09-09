using System.Text.RegularExpressions;

namespace Conduit.Core.Articles
{
    public class Slug
    {
        public Slug(string value)
        {
            Value = Slugify(value);
        }

        public string Value { get; }

        private string Slugify(string value)
        {
            return Regex.Replace(value, @"\s", "-").ToLower();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(Slug slug)
        {
            return slug.Value;
        }
    }
}
