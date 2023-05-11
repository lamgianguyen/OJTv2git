using System;

namespace OJTv2.Requests
{
    public class SemesterRequest
    {
        public string SemesterId { get; set; }
        public string SemesterName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StatusId { get; set; }
        
    }
}
