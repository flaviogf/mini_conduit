using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class EditUser
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public string Image { get; set; }

        public string Bio { get; set; }
    }
}
