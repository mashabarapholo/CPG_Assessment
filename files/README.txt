====================================================
  Capital Peak Group - C# Intern Development Test
  Login Application - README
====================================================

HOW TO RUN
----------
1. Open Visual Studio (2019 or later recommended).
2. Open the solution file: LoginApp.sln
3. Build the solution (Ctrl+Shift+B).
4. Press F5 to run.

Requirements: .NET Framework 4.7.2 (pre-installed on most Windows machines).
No external NuGet packages required - hashing uses built-in System.Security.Cryptography.

====================================================
TEST CREDENTIALS
====================================================
Username     | Password
-------------|-------------
admin        | Admin@123
alice        | Alice#456
bob_dev      | Bob$Dev7
intern1      | Intern@1

Note: Passwords are CASE-SENSITIVE.
Note: Passwords are stored as salted SHA256 hashes - plaintext is never stored.

====================================================
FEATURES IMPLEMENTED
====================================================

CORE FEATURES:
- Login Form with username/password fields
- Show/Hide password toggle (checkbox)
- Input validation:
    * Empty field detection
    * Username: 3-15 chars, alphanumeric + underscore only
    * Password: min 6 chars, must have uppercase, lowercase, digit, special char
    * No leading/trailing whitespace in password
- Account Lockout after 3 failed attempts
- Remaining attempts warning shown after each failure
- Locked accounts show clear error immediately (no credential check)
- Successful login resets the failed attempt counter
- Post-login Dashboard with welcome message + date/time
- Logout button returns to login screen and clears session

BONUS FEATURES:
- [BONUS] Admin Unlock Screen: Click "Admin Unlock Account" on login screen.
  Admin password: CPG_Admin@2024
- [BONUS] Password Hashing (SHA256 + Salt): Passwords NEVER stored as plaintext.
  Each password is hashed with a random 128-bit salt using SHA256.
  Constant-time comparison prevents timing attacks. See PasswordHasher.cs.
- [BONUS] Remember Me: Saves username to remembered_user.txt (persists restarts)
- [BONUS] Login Audit Log: All events saved to login_audit.txt in the app folder

====================================================
CODE STRUCTURE
====================================================
Program.cs           - Application entry point
LoginForm.cs         - Main login UI and event handling
DashboardForm.cs     - Post-login welcome screen
AdminUnlockForm.cs   - Admin account unlock screen (Bonus)
AuthService.cs       - Credential store and authentication logic
PasswordHasher.cs    - SHA256 + salt hashing and verification (Bonus)
ValidationHelper.cs  - All input validation rules (separate concern)
SessionState.cs      - Failed attempt tracking and lockout state
AuditLogger.cs       - Writes login events to login_audit.txt (Bonus)
RememberMeHelper.cs  - Saves/loads remembered username (Bonus)
LoginApp.csproj      - Project configuration
LoginApp.sln         - Visual Studio solution file

====================================================
DESIGN DECISIONS
====================================================
- Validation logic is kept in ValidationHelper.cs (separate from UI),
  following separation of concerns principle.
- SessionState uses static dictionaries to maintain lockout state for
  the entire session without requiring a database.
- AuthService uses a Dictionary<string, string> for the credential store
  as the minimum required approach. This can be swapped for SQLite by
  replacing the Authenticate() method.
- PascalCase used for all methods and class names; camelCase for locals.
- No magic numbers - MaxAttempts and AdminPassword are constants.
====================================================
