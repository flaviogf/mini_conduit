using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core;
using CSharpFunctionalExtensions;
using Moq;
using Xunit;

namespace Conduit.Test
{
    public class CreateArticleHandlerTest
    {
        [Fact]
        public async Task HandleShouldReturnSuccess()
        {
            var request = new CreateArticleRequest("How to train your dragon", "Lorem ipsum", "Lorem impsum", Enumerable.Empty<string>());

            var articleRepository = new Mock<IArticleRepository>();

            var handler = new CreateArticleHandler(articleRepository.Object);

            Result result = await handler.Handle(request, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task HandleShoulAddAnArticle()
        {
            var request = new CreateArticleRequest("How to train your dragon", "Lorem ipsum", "Lorem impsum", Enumerable.Empty<string>());

            var articleRepository = new Mock<IArticleRepository>();

            var handler = new CreateArticleHandler(articleRepository.Object);

            await handler.Handle(request, CancellationToken.None);

            articleRepository.Verify(it => it.Add(It.IsAny<Article>()), Times.Once);
        }

        [Fact]
        public async Task HandleShoulAddAnArticleWithThePassedTags()
        {
            var request = new CreateArticleRequest("How to train your dragon", "Lorem ipsum", "Lorem impsum", new string[] { "movie" });

            var articleRepository = new Mock<IArticleRepository>();

            Article article = null;

            articleRepository.Setup(it => it.Add(It.IsAny<Article>())).Callback<Article>(it => article = it);

            var handler = new CreateArticleHandler(articleRepository.Object);

            await handler.Handle(request, CancellationToken.None);

            Assert.Collection(article.Tags,
                (it) =>
                {
                    Assert.Equal("movie", it);
                });
        }
    }
}
