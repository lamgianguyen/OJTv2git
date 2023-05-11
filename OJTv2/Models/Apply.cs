using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OJTv2.Models
{
    public partial class Apply
    {
        public int ApplyId { get; set; }
        public string StudentId { get; set; }
        public DateTime ApplyDate { get; set; }
        public string BusinessId { get; set; }
        public string Cv { get; set; }
        public int? StatusApply { get; set; }
        [JsonIgnore]
        public virtual BusinessinSemester Business { get; set; }
        public virtual StudentinSemester Student { get; set; }
    }
}
