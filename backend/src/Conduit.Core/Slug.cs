using System.Text.RegularExpressions;

namespace Conduit.Core
{
    public class Slug
    {
        public Slug(string value)
        {
            Value = Slugify(value);
        }

        public string Value { get; }

        private string Slugify(string value) => Regex.Replace(value, @"\s", "-").ToLower();
    }
}
