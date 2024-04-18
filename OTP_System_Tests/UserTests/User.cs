using OTP_System_Server.Models;
using OTP_System_Server.Services;

namespace OTP_System_Tests.UserTests
{
    internal class User
    {
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _userService = new UserService();
        }

        [Test]
        public void RegisterUser_ValidUsernameAndPassword_ReturnsUserModel()
        {
            // Arrange
            string username = "testuser";
            string password = "testpassword";

            // Act
            UserModel user = _userService.RegisterUser(username, password);

            // Assert
            Assert.IsNotNull(user);
            Assert.That(username, Is.EqualTo(user.Username));
            Assert.IsNotNull(user.PasswordHash);
        }

        [Test]
        public void Authenticate_ValidCredentials_ReturnsUserModelWithTotpSecret()
        {
            // Arrange
            string username = "testuser";
            string password = "testpassword";
            _userService.RegisterUser(username, password);

            // Act
            UserModel authenticatedUser = _userService.Authenticate(username, password);

            // Assert
            Assert.IsNotNull(authenticatedUser);
            Assert.IsNotEmpty(authenticatedUser.TotpSecret);
            Assert.IsNotNull(authenticatedUser.TotpSecret);
        }

        [Test]
        public void ValidateOtp_ValidUsernameAndEncryptedOtp_ReturnsTrue()
        {
            // Arrange
            string username = "testuser";
            string password = "testpassword";
            UserModel user = _userService.RegisterUser(username, password);
            _userService.Authenticate(username, password); // To generate TotpSecret
            string encryptedOtp = SimpleEncryptionService.Encrypt(TotpGeneratorService.GenerateTotp(user.PasswordHash));

            // Act
            bool isValid = _userService.ValidateOtp(username, encryptedOtp);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void ValidateOtp_InvalidEncryptedOtp_ReturnsFalse()
        {
            // Arrange
            string username = "testuser";
            string password = "testpassword";
            _userService.RegisterUser(username, password);
            string encryptedOtp = "invalidencryptedotp";

            // Act
            bool isValid = _userService.ValidateOtp(username, encryptedOtp);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void ValidateOtp_InvalidUsername_ReturnsFalse()
        {
            // Arrange
            string username = "nonexistentuser";
            string encryptedOtp = "encryptedotp";

            // Act
            bool isValid = _userService.ValidateOtp(username, encryptedOtp);

            // Assert
            Assert.IsFalse(isValid);
        }
    }
}
