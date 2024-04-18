using System.Security.Cryptography;
using System.Text;

namespace OTP_System_Server.Services
{
    public static class EncryptionService
    {
        private static readonly string EncryptionKey = "ThisIsMyEncryptionKey";
        private static readonly byte[] Key = DeriveKey(EncryptionKey, 256);

        private static byte[] DeriveKey(string password, int keySize)
        {
            const int SaltSize = 16;
            const int Iterations = 10000;
            var hashAlgorithm = HashAlgorithmName.SHA1; 

            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Iterations, hashAlgorithm))
            {
                return deriveBytes.GetBytes(keySize / 8);
            }
        }

        public static string Encrypt(string plainText)
        {
            byte[] encryptedBytes;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = new byte[16];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(plainText);
                        csEncrypt.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            string plainText = null;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key; 
                aesAlg.IV = new byte[16]; 

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }
    }

}



