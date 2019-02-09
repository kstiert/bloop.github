using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Bloop.GitHub.Shell
{
    public class GitBash : IShell
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern bool PathFindOnPath([In, Out] StringBuilder pszFile, [In] string[] ppszOtherDirs);

        public bool Avalible
        {
            get
            {
                var exe = new StringBuilder("bash.exe");
                return PathFindOnPath(exe, new string[0]);
            }
        }

        public void Launch(string wd, string command = null)
        {
            var info = new ProcessStartInfo
            {
                FileName = "bash.exe",
                WorkingDirectory = wd
            };
            Process.Start(info);
        }
    }
}
