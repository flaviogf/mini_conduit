namespace Conduit.Api.Models
{
    public class ArticleTag
    {
        public Article Article { get; set; }

        public string ArticleId { get; set; }

        public Tag Tag { get; set; }

        public string TagId { get; set; }
    }
}
