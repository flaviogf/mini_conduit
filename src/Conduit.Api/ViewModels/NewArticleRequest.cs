using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class NewArticleRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public IList<string> Tags { get; set; } = new List<string>();
    }
}
