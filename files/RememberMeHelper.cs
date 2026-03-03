using System;
using System.IO;

namespace LoginApp
{
    public static class RememberMeHelper
    {
        private static readonly string FilePath = "rememberedUser.txt";

        public static void Save(string username)
        {
            try { File.WriteAllText(FilePath, username); }
            catch { }
        }

        public static string Load()
        {
            try
            {
                if (File.Exists(FilePath))
                    return File.ReadAllText(FilePath).Trim();
            }
            catch { }
            return string.Empty;
        }

        public static void Clear()
        {
            try
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
            }
            catch { }
        }
    }
}
