using System;
using System.IO;

namespace LoginApp
{
    public static class AuditLogger
    {
        private static readonly string LogPath = "loginAudit.txt";

        public static void Log(string username, bool success, string notes = "")
        {
            try
            {
                string status = success ? "SUCCESS" : "FAIL";
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string entry = $"{timestamp} | User: {username} | Status: {status} | Notes: {notes}";
                File.AppendAllText(LogPath, entry + Environment.NewLine);
            }
            catch {}
        }
    }
}
