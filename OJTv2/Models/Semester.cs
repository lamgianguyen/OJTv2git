using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OJTv2.Models
{
    public partial class Semester
    {
        public Semester()
        {
            BusinessinSemesters = new HashSet<BusinessinSemester>();
            StudentinSemesters = new HashSet<StudentinSemester>();
        }

        public string SemesterId { get; set; }
        public string SemesterName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }
        [JsonIgnore]
        public virtual ICollection<BusinessinSemester> BusinessinSemesters { get; set; }
        public virtual ICollection<StudentinSemester> StudentinSemesters { get; set; }
    }
}
