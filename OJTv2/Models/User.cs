using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OJTv2.Models
{
    public partial class User
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public virtual Role Role { get; set; }
        public virtual Business Business { get; set; }
        public virtual Student Student { get; set; }
    }
}
