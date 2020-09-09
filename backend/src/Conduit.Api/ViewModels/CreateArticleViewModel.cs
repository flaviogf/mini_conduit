using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class CreateArticleViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public IEnumerable<string> TagList { get; set; } = new List<string>();
    }
}
