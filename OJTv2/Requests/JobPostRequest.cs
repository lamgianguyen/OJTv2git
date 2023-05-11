using System.Collections.Generic;

namespace OJTv2.Requests
{
    public class JobPostRequest
    {
        public string businessID { get; set; }
        public int JobPositionId { get; set; }
        public string JobName { get; set; }
        public string DetailWork { get; set; }
        public string Request { get; set; }
        public int? Salary { get; set; }
        public string WorkLocation { get; set; }
        public string DetailBusiness { get; set; }
        public string Benefit { get; set; }
        public int? Amount { get; set; }
        public List<string> ListDepartment { get; set; }
    }
}
