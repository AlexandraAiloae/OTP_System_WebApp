using OTP_System_Server.Models;

namespace OTP_System_Server.Services
{
    public interface IUserService
    {
        UserModel RegisterUser(string username, string password);
        UserModel Authenticate(string username, string password);
        bool ValidateOtp(string username, string otp);
    }

    public class UserService : IUserService
    {
        private readonly List<UserModel> _users = new List<UserModel>();

        public UserModel RegisterUser(string username, string password)
        {
            var passwordHash = HashPassword(password);

            var user = new UserModel
            {
                Username = username,
                PasswordHash = passwordHash,
            };

            _users.Add(user);
            return user;
        }

        public UserModel Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && VerifyPassword(x.PasswordHash, password));
            if (user != null)
            {
                //user.TotpSecret = EncryptionService.Encrypt(TotpGeneratorService.GenerateTotp(user.PasswordHash));
                user.TotpSecret = SimpleEncryptionService.Encrypt(TotpGeneratorService.GenerateTotp(user.PasswordHash));
            }
            return user;
        }

        public bool ValidateOtp(string username, string encryptedOtp)
        {
            var user = _users.SingleOrDefault(x => x.Username == username);
            if (user == null)
                return false;

            //var otp = EncryptionService.Decrypt(encryptedOtp);
            var otp = SimpleEncryptionService.Decrypt(encryptedOtp);

            return TotpGeneratorService.ValidateTotp(user.PasswordHash, otp);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        private bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }


    }
}
