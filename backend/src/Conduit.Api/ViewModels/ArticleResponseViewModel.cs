namespace Conduit.Api.ViewModels
{
    public class ArticleResponseViewModel
    {
        public ArticleResponseViewModel(ArticleViewModel article)
        {
            Article = article;
        }

        public ArticleViewModel Article { get; }
    }
}
