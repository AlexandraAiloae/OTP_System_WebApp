using OTP_System_Server.Services;

namespace OTP_System_Tests.TotpTests
{
    internal class Totp
    {
        [Test]
        public void GenerateTotp_ValidSecret_Returns6Digits()
        {
            // Arrange
            string secret = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

            // Act
            string otp = TotpGeneratorService.GenerateTotp(secret);

            // Assert
            Assert.That(6, Is.EqualTo(otp.Length));
        }

        [Test]
        public void ValidateTotp_ValidOtp_ReturnsTrue()
        {
            // Arrange
            string secret = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            string otp = TotpGeneratorService.GenerateTotp(secret);

            // Act
            bool isValid = TotpGeneratorService.ValidateTotp(secret, otp);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void ValidateTotp_InvalidOtp_ReturnsFalse()
        {
            // Arrange
            string secret = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            string otp = "123456"; // Invalid OTP

            // Act
            bool isValid = TotpGeneratorService.ValidateTotp(secret, otp);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void ValidateTotp_ExpiredOtp_ReturnsFalse()
        {
            // Arrange
            string secret = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            string otp = TotpGeneratorService.GenerateTotp(secret);

            // Simulate time passage
            System.Threading.Thread.Sleep(61000);

            // Act
            bool isValid = TotpGeneratorService.ValidateTotp(secret, otp);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void ValidateTotp_ValidOtpWithTolerance_ReturnsTrue()
        {
            // Arrange
            string secret = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            string otp = TotpGeneratorService.GenerateTotp(secret);

            // Simulate time passage within tolerance window
            System.Threading.Thread.Sleep(15000);

            // Act
            bool isValid = TotpGeneratorService.ValidateTotp(secret, otp);

            // Assert
            Assert.IsTrue(isValid);
        }
    }
}
