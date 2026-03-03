using System.Collections.Generic;

namespace LoginApp
{
   
    public static class AuthService
    {
        // Hashed credentials generated at startup via PasswordHasher.Hash()
        // Each run produces a different hash (different salt) — this is intentional.
        private static readonly Dictionary<string, string> HashedCredentials = BuildCredentials();

        private static Dictionary<string, string> BuildCredentials()
        {
            // Hash each plaintext password once at startup.
            return new Dictionary<string, string>
            {
                { "admin",   PasswordHasher.Hash("Admin@246") },
                { "mary",   PasswordHasher.Hash("Mary#456") },
                { "Mashaba77", PasswordHasher.Hash("Mash$AI7")  },
                { "Daniel", PasswordHasher.Hash("Daniel@1")  }
            };
        }
        /// Authenticates a user by verifying the password against its stored hash.
        /// Password comparison is done securely using constant-time hash verification.
        public static bool Authenticate(string username, string password)
        {
            if (HashedCredentials.TryGetValue(username, out string storedHash))
            {
                return PasswordHasher.Verify(password, storedHash);
            }
            return false;
        }
        /// Returns true if the username exists in the credential store.
        
        public static bool UserExists(string username)
        {
            return HashedCredentials.ContainsKey(username);
        }
    }
}
