using Conduit.Core;
using Xunit;

namespace Conduit.Test
{
    public class SlugTest
    {
        [Fact]
        public void ValueShouldReturnThePassedValueSlugified()
        {
            var slug = new Slug("How to train your dragon");

            Assert.Equal("how-to-train-your-dragon", slug.Value);
        }

        [Fact]
        public void ToStringShouldReturnTheValueOfSlug()
        {
            var slug = new Slug("How to train your dragon");

            Assert.Equal("how-to-train-your-dragon", slug.ToString());
        }

        [Fact]
        public void CastToStringShouldReturnTheValueOfSlug()
        {
            var slug = new Slug("How to train your dragon");

            Assert.Equal("how-to-train-your-dragon", slug);
        }
    }
}
