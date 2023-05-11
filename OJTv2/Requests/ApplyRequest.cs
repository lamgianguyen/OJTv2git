using System;

namespace OJTv2.Requests
{
    public class ApplyRequest
    {
       
        public string StudentId { get; set; }
       
        public string Cv { get; set; }
        public int StatusApply { get; set; }
    }
}
