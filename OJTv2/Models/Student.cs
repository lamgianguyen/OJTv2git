using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OJTv2.Models
{
    public partial class Student
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string SemesterId { get; set; }
        public string DeparmentId { get; set; }
        [JsonIgnore]
        public virtual Department Deparment { get; set; }
        public virtual User StudentNavigation { get; set; }
        public virtual StudentinSemester StudentinSemester { get; set; }
    }
}
