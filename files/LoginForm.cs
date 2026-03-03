using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LoginApp
{
    public class LoginForm : Form
    {
        // UI Controls
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private CheckBox chkRememberMe;
        private Button btnLogin;
        private Button btnAdminUnlock;
        private Label lblStatus;
        private Panel pnlHeader;
        private Label lblCompany;

        public LoginForm()
        {
            InitializeComponents();
            LoadRememberedUsername();
        }

        private void InitializeComponents()
        {
            this.Text = "Capital Peak Group – Login";
            this.Size = new Size(420, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Header Panel
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(26, 58, 108)
            };

            lblCompany = new Label
            {
                Text = "CAPITAL PEAK GROUP\r\nWhen you work with Capital Peak, you tap into a deep and diverse pool of talent, education, and expertise.",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            pnlHeader.Controls.Add(lblCompany);

            // Username
            lblUsername = new Label
            {
                Text = "Username",
                Location = new Point(60, 110),
                Size = new Size(290, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 58, 108)
            };

            txtUsername = new TextBox
            {
                Location = new Point(60, 133),
                Size = new Size(290, 28),
                Font = new Font("Segoe UI", 10),
                MaxLength = 15
            };

            // Password
            lblPassword = new Label
            {
                Text = "Password",
                Location = new Point(60, 175),
                Size = new Size(290, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 58, 108)
            };

            txtPassword = new TextBox
            {
                Location = new Point(60, 198),
                Size = new Size(290, 28),
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true
            };

            // Show Password
            chkShowPassword = new CheckBox
            {
                Text = "Show Password",
                Location = new Point(60, 232),
                Size = new Size(150, 22),
                Font = new Font("Segoe UI", 9)
            };
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;

            // Remember Me
            chkRememberMe = new CheckBox
            {
                Text = "Remember Me",
                Location = new Point(220, 232),
                Size = new Size(150, 22),
                Font = new Font("Segoe UI", 9)
            };

            // Status Label
            lblStatus = new Label
            {
                Location = new Point(60, 268),
                Size = new Size(290, 50),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Login Button
            btnLogin = new Button
            {
                Text = "LOGIN",
                Location = new Point(60, 325),
                Size = new Size(290, 40),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(26, 58, 108),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Admin Unlock Button
            btnAdminUnlock = new Button
            {
                Text = "Admin Unlock Account",
                Location = new Point(60, 378),
                Size = new Size(290, 35),
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAdminUnlock.FlatAppearance.BorderSize = 0;
            btnAdminUnlock.Click += BtnAdminUnlock_Click;

            // Footer
            Label lblFooter = new Label
            {
                Text = "© Capital Peak Group – Confidential",
                Dock = DockStyle.Bottom,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };

            this.Controls.AddRange(new Control[]
            {
                pnlHeader, lblUsername, txtUsername,
                lblPassword, txtPassword, chkShowPassword,
                chkRememberMe, lblStatus, btnLogin,
                btnAdminUnlock, lblFooter
            });

            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // Validate inputs
            string validationError = ValidationHelper.Validate(username, password);
            if (!string.IsNullOrEmpty(validationError))
            {
                ShowStatus(validationError, Color.Red);
                return;
            }

            // Check lockout
            if (SessionState.IsLocked(username))
            {
                ShowStatus("Account locked. Too many failed attempts.\nPlease contact your administrator.", Color.Red);
                AuditLogger.Log(username, false, "Account locked – login attempt blocked");
                return;
            }

            // Authenticate
            if (AuthService.Authenticate(username, password))
            {
                SessionState.ResetAttempts(username);

                if (chkRememberMe.Checked)
                    RememberMeHelper.Save(username);
                else
                    RememberMeHelper.Clear();

                AuditLogger.Log(username, true, "Successfully logged in");

                DashboardForm dashboard = new DashboardForm(username);
                dashboard.FormClosed += (s, args) => this.Show();
                this.Hide();
                dashboard.Show();
            }
            else
            {
                SessionState.RecordFailedAttempt(username);
                int remaining = SessionState.RemainingAttempts(username);

                if (SessionState.IsLocked(username))
                {
                    ShowStatus("Account locked. Too many failed attempts.\nPlease contact your administrator.", Color.Red);
                    AuditLogger.Log(username, false, "Account locked after 3rd failed attempt");
                }
                else
                {
                    ShowStatus($"Invalid credentials. {remaining} attempt{(remaining == 1 ? "" : "s")} remaining.", Color.OrangeRed);
                    AuditLogger.Log(username, false, $"Failed login – {remaining} attempts remaining");
                }
            }
        }

        private void BtnAdminUnlock_Click(object sender, EventArgs e)
        {
            AdminUnlockForm unlockForm = new AdminUnlockForm();
            unlockForm.ShowDialog(this);
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void ShowStatus(string message, Color color)
        {
            lblStatus.ForeColor = color;
            lblStatus.Text = message;
        }

        private void LoadRememberedUsername()
        {
            string remembered = RememberMeHelper.Load();
            if (!string.IsNullOrEmpty(remembered))
            {
                txtUsername.Text = remembered;
                chkRememberMe.Checked = true;
                txtPassword.Focus();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(278, 244);
            this.Name = "LoginForm";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
