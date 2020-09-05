using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class StoreUserRequest
    {
        [Required]
        public StoreUser User { get; set; }
    }
}
