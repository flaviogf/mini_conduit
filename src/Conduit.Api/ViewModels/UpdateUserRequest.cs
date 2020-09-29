using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class UpdateUserRequest
    {
        public string Avatar { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Bios { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
