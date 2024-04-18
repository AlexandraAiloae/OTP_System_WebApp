using OTP_System_Server.Services;

namespace OTP_System_Tests.EncyptionTests
{
    internal class SimpleEncryption
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EncryptDecrypt_SameString_Success()
        {
            // Arrange
            string original = "password";

            // Act
            string encrypted = SimpleEncryptionService.Encrypt(original);
            string decrypted = SimpleEncryptionService.Decrypt(encrypted);

            // Assert
            Assert.That(decrypted, Is.EqualTo(original));
        }

        [Test]
        public void EncryptDecrypt_EmptyString_Success()
        {
            // Arrange
            string original = "";

            // Act
            string encrypted = SimpleEncryptionService.Encrypt(original);
            string decrypted = SimpleEncryptionService.Decrypt(encrypted);

            // Assert
            Assert.That(decrypted, Is.EqualTo(original));
        }

        [Test]
        public void EncryptDecrypt_LongString_Success()
        {
            // Arrange
            string original = "thisisaverylongpasswordwithmultiplecharacters";

            // Act
            string encrypted = SimpleEncryptionService.Encrypt(original);
            string decrypted = SimpleEncryptionService.Decrypt(encrypted);

            // Assert
            Assert.That(decrypted, Is.EqualTo(original));
        }

        [Test]
        public void EncryptDecrypt_SpecialCharacters_Success()
        {
            // Arrange
            string original = "!@#$%^&*()_+{}:\"<>?";

            // Act
            string encrypted = SimpleEncryptionService.Encrypt(original);
            string decrypted = SimpleEncryptionService.Decrypt(encrypted);

            // Assert
            Assert.That(decrypted, Is.EqualTo(original));
        }
    }
}
