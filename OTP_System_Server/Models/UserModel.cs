namespace OTP_System_Server.Models
{
    public class UserModel
    {
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public string? TotpSecret { get; set; }
    }
}
