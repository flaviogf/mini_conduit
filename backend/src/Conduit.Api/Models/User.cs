using System.Text.Json.Serialization;

namespace Conduit.Api.Models
{
    public class User
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public string Token { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }
    }
}
