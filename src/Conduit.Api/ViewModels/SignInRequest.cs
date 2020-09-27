using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class SignInRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
