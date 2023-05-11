using System;

namespace OJTv2.Requests
{
    public class ApplyinJobPost
    {
        public int ApplyId { get; set; }
        public string StudentName { get; set; }
        public string StudentId { get; set; }
        public DateTime ApplyDate { get; set; }
        public string BusinessId { get; set; }
        public string Cv { get; set; }
        public int? StatusApply { get; set; }
    }
}
