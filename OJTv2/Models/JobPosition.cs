using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OJTv2.Models
{
    public partial class JobPosition
    {
        public JobPosition()
        {
            BusinessinSemesters = new HashSet<BusinessinSemester>();
            JobDepartments = new HashSet<JobDepartment>();
        }

        public int JobPositionId { get; set; }
        public string JobName { get; set; }
        public string DetailWork { get; set; }
        public string Request { get; set; }
        public int? Salary { get; set; }
        public string WorkLocation { get; set; }
        public string DetailBusiness { get; set; }
        public string Benefit { get; set; }
        public int? Amount { get; set; }
        [JsonIgnore]
        public virtual ICollection<BusinessinSemester> BusinessinSemesters { get; set; }
        public virtual ICollection<JobDepartment> JobDepartments { get; set; }
    }
}
