using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class StoreUserLogin
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
