using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class NewUserRequest
    {
        [Required]
        public NewUser User { get; set; }
    }
}
