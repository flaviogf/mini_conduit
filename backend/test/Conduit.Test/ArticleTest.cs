using System.Linq;
using Conduit.Core;
using Xunit;

namespace Conduit.Test
{
    public class ArticleTest
    {
        [Fact]
        public void SlugShouldBeTheTitleOfTheArticleSlugified()
        {
            var article = new Article("How to train your dragon", "Lorem ipsum", "Lorem ipsum");

            Assert.Equal("how-to-train-your-dragon", article.Slug);
        }

        [Fact]
        public void TagsShouldBeAnEmptyList()
        {
            var article = new Article("How to train your dragon", "Lorem ipsum", "Lorem ipsum");

            Assert.False(article.Tags.Any());
        }

        [Fact]
        public void AddTagsShouldAddPassedTags()
        {
            var article = new Article("How to train your dragon", "Lorem ipsum", "Lorem ipsum");

            string[] tagList = new string[] { "movie", "animation" };

            article.AddTags(tagList);

            Assert.Collection(article.Tags,
                (it) =>
                {
                    Assert.Equal("movie", it);
                },
                (it) =>
                {
                    Assert.Equal("animation", it);
                });
        }
    }
}
