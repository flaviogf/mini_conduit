using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class StoreArticle
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Body { get; set; }

        public IEnumerable<string> TagList { get; set; }
    }
}
