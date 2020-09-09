using Conduit.Core.Articles;
using Xunit;

namespace Conduit.Test.Articles
{
    public class TagTest
    {
        [Fact]
        public void ToStringShouldReturnTheNameOfTheTag()
        {
            var tag = new Tag("movie");

            Assert.Equal("movie", tag.ToString());
        }

        [Fact]
        public void CastToStringShouldReturnTheNameOfTheTag()
        {
            var tag = new Tag("movie");

            Assert.Equal("movie", tag);
        }
    }
}
