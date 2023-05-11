namespace OJTv2.Models
{
    public class UserJWT
    {
        public string UserID { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }


    }
}
