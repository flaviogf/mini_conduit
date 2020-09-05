using System.Runtime.Serialization;

namespace Conduit.Api.Models
{
    [DataContract]
    public class User
    {
        public string Id { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public string Bio { get; set; }

        [DataMember]
        public string Image { get; set; }
    }
}
