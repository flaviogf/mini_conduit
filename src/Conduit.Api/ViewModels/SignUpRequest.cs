using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class SignUpRequest
    {
        [Required]
        public string UserName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
