using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OJTv2.Models
{
    public partial class JobDepartment
    {
        public int JobDepartmentId { get; set; }
        public int JobPostionId { get; set; }
        public string DepartmentId { get; set; }


        [JsonIgnore]
        public virtual Department Department { get; set; }
        public virtual JobPosition JobPostion { get; set; }
    }
}
