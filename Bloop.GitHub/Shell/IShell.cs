using System.Collections.Generic;

namespace Bloop.GitHub.Shell
{
    public interface IShell
    {
        bool Avalible { get; }

        string Name { get; }

        void Launch(string wd, List<string> commands = null);
    }
}
