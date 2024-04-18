using System;
using System.Security.Cryptography;

namespace OTP_System_Server.Services
{
    public class TotpGeneratorService
    {
        private const int DigitLength = 6;
        private const int TimeStepSeconds = 30;
        private const int ToleranceWindow = 1;

        public static string GenerateTotp(string secret)
        {
            long timeStepCounter = GetCurrentCounter();
            byte[] secretBytes = Base32Encoding.ToBytes(secret);

            byte[] counter = BitConverter.GetBytes(timeStepCounter);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counter);
            }

            HMACSHA1 hmac = new HMACSHA1(secretBytes);
            byte[] hash = hmac.ComputeHash(counter);

            int offset = hash[hash.Length - 1] & 0x0F;
            int binary =
                ((hash[offset] & 0x7f) << 24)
                | ((hash[offset + 1] & 0xff) << 16)
                | ((hash[offset + 2] & 0xff) << 8)
                | (hash[offset + 3] & 0xff);

            int otp = binary % (int)Math.Pow(10, DigitLength);

            return otp.ToString().PadLeft(DigitLength, '0');
        }

        public static bool ValidateTotp(string secret, string otp)
        {
            long timeStepCounter = GetCurrentCounter();
            byte[] secretBytes = Base32Encoding.ToBytes(secret);

            for (int i = -ToleranceWindow; i <= ToleranceWindow; i++)
            {
                long counter = timeStepCounter + i;
                byte[] counterBytes = BitConverter.GetBytes(counter);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(counterBytes);
                }

                HMACSHA1 hmac = new HMACSHA1(secretBytes);
                byte[] hash = hmac.ComputeHash(counterBytes);

                int offset = hash[hash.Length - 1] & 0x0F;
                int binary =
                    ((hash[offset] & 0x7f) << 24)
                    | ((hash[offset + 1] & 0xff) << 16)
                    | ((hash[offset + 2] & 0xff) << 8)
                    | (hash[offset + 3] & 0xff);

                int totp = binary % (int)Math.Pow(10, DigitLength);

                if (totp.ToString().PadLeft(DigitLength, '0') == otp)
                {
                    return true;
                }
            }

            return false;
        }

        private static long GetCurrentCounter()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds / TimeStepSeconds;
        }
    }

    public static class Base32Encoding
    {
        private const string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static byte[] ToBytes(string input)
        {
            input = input.TrimEnd('=');
            int byteCount = input.Length * 5 / 8;
            byte[] returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            int mask = 0, arrayIndex = 0;

            foreach (char c in input)
            {
                int cValue = Base32Chars.IndexOf(c);
                if (cValue < 0)
                    continue;

                if (bitsRemaining > 5)
                {
                    mask = cValue << (bitsRemaining - 5);
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> (5 - bitsRemaining);
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            return returnArray;
        }
    }
}
