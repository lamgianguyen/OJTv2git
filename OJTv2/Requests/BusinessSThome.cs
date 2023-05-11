namespace OJTv2.Requests
{
    public class BusinessSThome
    {
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string Image { get; set; }

        public int JobPositionId { get; set; }
       
        public int? Salary { get; set; }

        public string WorkLocation { get; set; }

        public int? Amount { get; set; }

    }
}
