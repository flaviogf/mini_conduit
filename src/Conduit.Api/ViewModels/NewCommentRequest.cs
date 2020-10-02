using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class NewCommentRequest
    {
        [Required]
        public string Text { get; set; }
    }
}
