using System;
using System.Security.Cryptography;
using System.Text;

namespace LoginApp { 

    public static class PasswordHasher
    {
        private const int SaltSize = 16; // 128-bit salt
        private const string Prefix = "sha256";
        /// Hashes a plaintext password and returns a storable hash string.
       
        public static string Hash(string password)
        {
            byte[] saltBytes = GenerateSalt();
            byte[] hashBytes = ComputeHash(password, saltBytes);

            string saltHex = BytesToHex(saltBytes);
            string hashHex = BytesToHex(hashBytes);

            return $"{Prefix}:{saltHex}:{hashHex}";
        }

        /// <summary>
        /// Verifies a plaintext password against a stored hash string.
        /// </summary>
        public static bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash) || !storedHash.StartsWith(Prefix + ":"))
                return false;

            string[] parts = storedHash.Split(':');
            if (parts.Length != 3)
                return false;

            byte[] saltBytes = HexToBytes(parts[1]);
            byte[] expectedHash = HexToBytes(parts[2]);
            byte[] actualHash = ComputeHash(password, saltBytes);

            return SlowEquals(expectedHash, actualHash);
        }

        // --- Private helpers ---

        private static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return salt;
            }
        }

        private static byte[] ComputeHash(string password, byte[] salt)
        {
            // Combine password bytes + salt, then SHA256
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combined = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, combined, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, combined, passwordBytes.Length, salt.Length);

            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(combined);
            }
        }

        /// <summary>
        /// Constant-time comparison to prevent timing attacks.
        /// </summary>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            int diff = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }

        private static string BytesToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }

        private static byte[] HexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }
    }
}
