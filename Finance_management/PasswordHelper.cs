using System;
using System.Security.Cryptography;
using System.Text;

namespace Finance_management
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;
        private const string Prefix = "PBKDF2:";

        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = DeriveKey(password, salt);
            return Prefix + Iterations + ":" +
                   Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            if (!storedHash.StartsWith(Prefix))
            {
                return storedHash == password;
            }

            string[] parts = storedHash.Substring(Prefix.Length).Split(':');
            if (parts.Length != 3)
            {
                return false;
            }

            int iterations = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] expected = Convert.FromBase64String(parts[2]);
            byte[] actual = DeriveKey(password, salt, iterations);

            return SlowEquals(expected, actual);
        }

        public static bool NeedsRehash(string storedHash)
        {
            return string.IsNullOrEmpty(storedHash) || !storedHash.StartsWith(Prefix);
        }

        private static byte[] DeriveKey(string password, byte[] salt)
        {
            return DeriveKey(password, salt, Iterations);
        }

        private static byte[] DeriveKey(string password, byte[] salt, int iterations)
        {
            using (var derive = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return derive.GetBytes(HashSize);
            }
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }
}
