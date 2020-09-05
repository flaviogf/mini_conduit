using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class EditUserRequest
    {
        [Required]
        public EditUser User { get; set; }
    }
}
