using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Bloop.GitHub.Shell
{
    public class GitBash : IShell
    {
        private const string DEFAULT_PATH = "C:\\Program Files\\Git\\bin\\bash.exe";

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern bool PathFindOnPath([In, Out] StringBuilder pszFile, [In] string[] ppszOtherDirs);

        public string Path
        {
            get
            {
                if (File.Exists(DEFAULT_PATH))
                {
                    return DEFAULT_PATH;
                }

                var exe = new StringBuilder("bash.exe");
                if(PathFindOnPath(exe, null))
                {
                    return exe.ToString();
                }

                return null;
            }
        }

        public bool Avalible
        {
            get
            {
                return !string.IsNullOrEmpty(Path);
            }
        }

        public string Name => "Git Bash";

        public void Launch(string wd, List<string> commands)
        {
            var info = new ProcessStartInfo
            {
                FileName = Path,
                WorkingDirectory = wd
            };

            if(commands != null && commands.Any())
            {
                var tempPath = System.IO.Path.GetTempFileName();
                using (var file = File.OpenWrite(tempPath))
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(string.Join(";", commands));
                }

                info.Arguments = $"--init-file {tempPath}";
            }

            Process.Start(info);
        }
    }
}
