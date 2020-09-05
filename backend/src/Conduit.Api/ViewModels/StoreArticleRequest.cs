using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class StoreArticleRequest
    {
        [Required]
        public StoreArticle Article { get; set; }
    }
}
