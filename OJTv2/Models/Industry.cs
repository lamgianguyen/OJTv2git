using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OJTv2.Models
{
    public partial class Industry
    {
        public Industry()
        {
            Businesses = new HashSet<Business>();
        }

        public string IndustryId { get; set; }
        public string IndustryName { get; set; }
        [JsonIgnore]
        public virtual ICollection<Business> Businesses { get; set; }
    }
}
