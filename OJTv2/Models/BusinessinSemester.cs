using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OJTv2.Models
{
    public partial class BusinessinSemester
    {
        public BusinessinSemester()
        {
            Applies = new HashSet<Apply>();
        }

        public string BusinessId { get; set; }
        public string SemesterId { get; set; }
        public int? JobPositionId { get; set; }
        [JsonIgnore]
        public virtual Business Business { get; set; }
        public virtual JobPosition JobPosition { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual ICollection<Apply> Applies { get; set; }
    }
}
