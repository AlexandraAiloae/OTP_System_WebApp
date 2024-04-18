using System.Text;

namespace OTP_System_Server.Services
{
    public static class EncryptionConfig
    {
        public static readonly string Key = "ThisIsMyEncryptionKey";
    }

    public static class SimpleEncryptionService
    {
        public static string Encrypt(string input)
        {
            var key = EncryptionConfig.Key;
            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
                sb.Append((char)(input[i] ^ key[(i % key.Length)]));
            return sb.ToString();
        }

        public static string Decrypt(string input)
        {
            return Encrypt(input);
        }
    }
}
