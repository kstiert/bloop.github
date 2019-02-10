using System.Diagnostics;

namespace Bloop.GitHub.Shell
{
    public class Powershell : IShell
    {
        public bool Avalible => true;

        public string Name => "Powershell";

        public void Launch(string wd, string command = null)
        {
            var info = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                WorkingDirectory = wd
            };
            Process.Start(info);
        }
    }
}
