using System;
using System.Drawing;
using System.Windows.Forms;

namespace LoginApp
{
    public class AdminUnlockForm : Form
    {
        private const string AdminPassword = "CPGroup_Admin@2024";
        private TextBox txtAdminPass;
        private TextBox txtTargetUser;
        private Label lblStatus;

        public AdminUnlockForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Admin - Unlock Account";
            this.Size = new Size(380, 320);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.FromArgb(220, 53, 69)
            };

            Label lblTitle = new Label
            {
                Text = "Admin Account Unlock",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            pnlHeader.Controls.Add(lblTitle);

            Label lblAdminPass = new Label
            {
                Text = "Admin Password:",
                Location = new Point(30, 75),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            txtAdminPass = new TextBox
            {
                Location = new Point(30, 98),
                Size = new Size(300, 26),
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true
            };

            Label lblTargetUser = new Label
            {
                Text = "Username to Unlock:",
                Location = new Point(30, 138),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            txtTargetUser = new TextBox
            {
                Location = new Point(30, 161),
                Size = new Size(300, 26),
                Font = new Font("Segoe UI", 10)
            };

            lblStatus = new Label
            {
                Location = new Point(30, 198),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnUnlock = new Button
            {
                Text = "UNLOCK ACCOUNT",
                Location = new Point(30, 235),
                Size = new Size(300, 38),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(26, 58, 108),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnUnlock.FlatAppearance.BorderSize = 0;
            btnUnlock.Click += BtnUnlock_Click;

            this.Controls.AddRange(new Control[]
            {
                pnlHeader, lblAdminPass, txtAdminPass,
                lblTargetUser, txtTargetUser, lblStatus, btnUnlock
            });
        }

        private void BtnUnlock_Click(object sender, EventArgs e)
        {
            if (txtAdminPass.Text != AdminPassword)
            {
                ShowStatus("Incorrect admin password.", Color.Red);
                return;
            }

            string targetUser = txtTargetUser.Text.Trim();
            if (string.IsNullOrEmpty(targetUser))
            {
                ShowStatus("Please enter a username to unlock.", Color.Red);
                return;
            }

            bool unlocked = SessionState.UnlockAccount(targetUser);
            if (unlocked)
            {
                AuditLogger.Log(targetUser, true, "Account unlocked by admin");
                ShowStatus("Account unlocked successfully.", Color.Green);
            }
            else
            {
                ShowStatus("Account is not locked or does not exist.", Color.OrangeRed);
            }
        }

        private void ShowStatus(string message, Color color)
        {
            lblStatus.ForeColor = color;
            lblStatus.Text = message;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AdminUnlockForm
            // 
            this.ClientSize = new System.Drawing.Size(486, 325);
            this.Name = "AdminUnlockForm";
            this.ResumeLayout(false);

        }
    }
}
