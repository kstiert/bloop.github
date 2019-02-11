using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bloop.GitHub.Shell
{
    public class Powershell : IShell
    {
        public bool Avalible => true;

        public string Name => "Powershell";

        public void Launch(string wd, List<string> commands)
        {
            var info = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                WorkingDirectory = wd
            };

            if (commands != null && commands.Any())
            {
                info.Arguments = $"-NoExit -Command \"{string.Join(";", commands)}\"";
            }

            Process.Start(info);
        }
    }
}
