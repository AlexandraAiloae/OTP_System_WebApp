using OTP_System_Server.Services;

namespace OTP_System_Tests.EncyptionTests
{
    internal class Encryption
    {
        [Test]
        public void EncryptDecrypt_SameString_Success()
        {
            // Arrange
            string original = "password";

            // Act
            string encrypted = EncryptionService.Encrypt(original);
            string decrypted = EncryptionService.Decrypt(encrypted);

            // Assert
            Assert.That(original, Is.EqualTo(decrypted));
        }

        [Test]
        public void EncryptDecrypt_EmptyString_Success()
        {
            // Arrange
            string original = "";

            // Act
            string encrypted = EncryptionService.Encrypt(original);
            string decrypted = EncryptionService.Decrypt(encrypted);

            // Assert
            Assert.That(original.Equals(decrypted));
        }

        [Test]
        public void EncryptDecrypt_LongString_Success()
        {
            // Arrange
            string original = "thisisaverylongstringwithmanychars";

            // Act
            string encrypted = EncryptionService.Encrypt(original);
            string decrypted = EncryptionService.Decrypt(encrypted);

            // Assert
            Assert.That(original, Is.EqualTo(decrypted));
        }

        [Test]
        public void EncryptDecrypt_SpecialCharacters_Success()
        {
            // Arrange
            string original = "!@#$%^&*()_+{}:\"<>?";

            // Act
            string encrypted = EncryptionService.Encrypt(original);
            string decrypted = EncryptionService.Decrypt(encrypted);

            // Assert
            Assert.That(original, Is.EqualTo(decrypted));
        }
    }
}
