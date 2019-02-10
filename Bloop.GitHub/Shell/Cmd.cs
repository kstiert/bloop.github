using System.Diagnostics;

namespace Bloop.GitHub.Shell
{
    public class Cmd : IShell
    {
        public bool Avalible => true;

        public string Name => "Cmd";

        public void Launch(string wd, string command = null)
        {
            var info = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = wd
            };
            Process.Start(info);
        }
    }
}
