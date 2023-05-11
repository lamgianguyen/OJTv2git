using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OJTv2.Models
{
    public partial class Business
    {
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string Website { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Image { get; set; }
        public string IndustryId { get; set; }
        public string SemesterId { get; set; }
        [JsonIgnore]
        public virtual User BusinessNavigation { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual BusinessinSemester BusinessinSemester { get; set; }
    }
}
