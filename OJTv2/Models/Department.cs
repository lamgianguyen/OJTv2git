using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OJTv2.Models
{
    public partial class Department
    {
        public Department()
        {
            JobDepartments = new HashSet<JobDepartment>();
            Students = new HashSet<Student>();
        }

        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        [JsonIgnore]
        public virtual ICollection<JobDepartment> JobDepartments { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
