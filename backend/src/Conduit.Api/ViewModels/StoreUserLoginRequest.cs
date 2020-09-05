using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.ViewModels
{
    public class StoreUserLoginRequest
    {
        [Required]
        public StoreUserLogin User { get; set; }
    }
}
