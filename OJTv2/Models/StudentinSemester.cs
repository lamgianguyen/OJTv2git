using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OJTv2.Models
{
    public partial class StudentinSemester
    {
        public StudentinSemester()
        {
            Applies = new HashSet<Apply>();
        }

        public string StudentId { get; set; }
        public string SemesterId { get; set; }
        [JsonIgnore]
        public virtual Semester Semester { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<Apply> Applies { get; set; }
    }
}
