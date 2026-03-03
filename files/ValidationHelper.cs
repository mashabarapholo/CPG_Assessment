using System.Text.RegularExpressions;

namespace LoginApp
{
    public static class ValidationHelper
    {
        // Validate username and password, return error message or empty string if valid
        public static string Validate(string username, string password)
        {
            //Empty fields
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
                return "Username and password are required.";

            //Username length (3–15 chars)
            if (username.Length < 3)
                return "Username must be at least 3 characters.";
            if (username.Length > 15)
                return "Username must be at most 15 characters.";

            //Username alphanumeric + underscores only
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
                return "Username may only contain letters, numbers, and underscores.";

            //Password minimum length
            if (password.Length < 6)
                return "Password must be at least 6 characters.";

            // No leading/trailing whitespace in password
            if (password != password.Trim())
                return "Password must not have leading or trailing spaces.";

            // Password complexity
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return "Password must contain at least 1 uppercase letter.";
            if (!Regex.IsMatch(password, @"[a-z]"))
                return "Password must contain at least 1 lowercase letter.";
            if (!Regex.IsMatch(password, @"[0-9]"))
                return "Password must contain at least 1 digit.";
            if (!Regex.IsMatch(password, @"[@#\$%\^&\*]"))
                return "Password must contain at least 1 special character (@#$%^&*).";

            return string.Empty;
        }
    }
}
