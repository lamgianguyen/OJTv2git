using System;

namespace OJTv2.Requests
{
    public class ApplyStudent
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }

        public string Status { get; set; }
        public string BusinessName { get; set; }
        public DateTime ApplyDate { get; set; }
    }
}
