using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class CreateArticleRequestViewModel
    {
        [Required]
        public CreateArticleViewModel Article { get; set; }
    }
}
