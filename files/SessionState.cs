using System.Collections.Generic;

namespace LoginApp
{
    public static class SessionState
    {
        private const int MaxAttempts = 3;
        private static readonly Dictionary<string, int> FailedAttempts = new Dictionary<string, int>();
        private static readonly HashSet<string> LockedAccounts = new HashSet<string>();

        public static void RecordFailedAttempt(string username)
        {
            if (!FailedAttempts.ContainsKey(username))
                FailedAttempts[username] = 0;
            FailedAttempts[username]++;
            if (FailedAttempts[username] >= MaxAttempts)
                LockedAccounts.Add(username);
        }

        public static bool IsLocked(string username) => LockedAccounts.Contains(username);

        public static int RemainingAttempts(string username)
        {
            int used = FailedAttempts.ContainsKey(username) ? FailedAttempts[username] : 0;
            int remaining = MaxAttempts - used;
            return remaining < 0 ? 0 : remaining;
        }

        public static void ResetAttempts(string username)
        {
            if (FailedAttempts.ContainsKey(username))
                FailedAttempts[username] = 0;
        }

        public static bool UnlockAccount(string username)
        {
            if (LockedAccounts.Contains(username))
            {
                LockedAccounts.Remove(username);
                if (FailedAttempts.ContainsKey(username))
                    FailedAttempts[username] = 0;
                return true;
            }
            return false;
        }
    }
}
