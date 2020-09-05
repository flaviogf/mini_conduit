using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Conduit.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        [NotMapped]
        public string Token { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }
    }
}
