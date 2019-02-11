using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bloop.GitHub.Shell
{
    public class Cmd : IShell
    {
        public bool Avalible => true;

        public string Name => "Cmd";

        public void Launch(string wd, List<string> commands)
        {
            var info = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = wd
            };

            if(commands != null && commands.Any())
            {
                info.Arguments = $"/K \"{string.Join(" & ", commands)}\"";
            }

            Process.Start(info);
        }
    }
}
