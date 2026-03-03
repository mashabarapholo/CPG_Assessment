using System;
using System.Drawing;
using System.Windows.Forms;

namespace LoginApp
{
    public class DashboardForm : Form
    {
        private string _username;

        public DashboardForm(string username)
        {
            _username = username;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Capital Peak Group – Dashboard";
            this.Size = new Size(500, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Header
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(26, 58, 108)
            };
            Label lblHeader = new Label
            {
                Text = "CAPITAL PEAK GROUP",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            pnlHeader.Controls.Add(lblHeader);

            // Welcome Message
            Label lblWelcome = new Label
            {
                Text = $"Welcome back, {_username}!",
                Location = new Point(0, 100),
                Size = new Size(500, 50),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 58, 108),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Date/Time of login
            Label lblDateTime = new Label
            {
                Text = $"Login time: {DateTime.Now:dddd, dd MMMM yyyy  HH:mm:ss}",
                Location = new Point(0, 155),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Logout Button
            Button btnLogout = new Button
            {
                Text = "LOGOUT",
                Location = new Point(175, 220),
                Size = new Size(150, 42),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) =>
            {
                AuditLogger.Log(_username, true, "User logged out");
                this.Close();
            };

            this.Controls.AddRange(new Control[] { pnlHeader, lblWelcome, lblDateTime, btnLogout });
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DashboardForm
            // 
            this.ClientSize = new System.Drawing.Size(278, 244);
            this.Name = "DashboardForm";
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            this.ResumeLayout(false);

        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {

        }
    }
}
