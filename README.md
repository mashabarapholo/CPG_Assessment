🚀 How To Run
Environment: Open the solution in Visual Studio 2019 or later.

Project File: Open LoginApp.sln.

Compile: Build the solution using Ctrl+Shift+B.

Execute: Press F5 to run the application.

Note: Requires .NET Framework 4.7.2. No external NuGet packages are required as hashing uses built-in cryptography libraries.

🔐 Test Credentials
Passwords are case-sensitive and stored as salted SHA256 hashes; plaintext is never stored.

✨ Features
Core Functionality
Secure Login: Username and password authentication.

UI Controls: Show/Hide password toggle via checkbox.

Strict Input Validation:

Detection of empty fields.

Username: 3-15 characters, alphanumeric and underscores only.

Password: Minimum 6 characters, requiring uppercase, lowercase, digits, and special characters.

Prevention of leading/trailing whitespace in passwords.

Account Lockout Policy:

Automatic lockout after 3 failed attempts.

Real-time warning of remaining attempts after each failure.

Immediate error messaging for locked accounts.

Successful login resets the attempt counter.

Post-Login Experience: Dashboard featuring a welcome message, current date/time, and a logout button that clears the session.

[BONUS] Advanced Features
Admin Unlock Screen: Provides a bypass to unlock accounts using the admin password: CPGroup_Admin@2024.

Password Hashing: Implements SHA256 + 128-bit Salt. Constant-time comparison is used to prevent timing attacks.

Remember Me: Persists the username to remembered_user.txt across application restarts.

Login Audit Log: Tracks all login events and failures in login_audit.txt.

🏗️ Project Structure
🛠️ Design Decisions
Separation of Concerns: Validation and hashing logic are kept in helper classes, separate from UI code.

Stateless Persistence: SessionState uses static dictionaries for temporary lockout tracking, while AuthService is designed to be easily swapped for a SQLite implementation.

Coding Standards: Adheres to PascalCase for methods/classes and camelCase for local variables.

Constants: Uses named constants for MaxAttempts and AdminPassword to avoid magic numbers.
