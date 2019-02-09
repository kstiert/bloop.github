using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloop.GitHub.Shell
{
    public interface IShell
    {
        bool Avalible { get; }

        void Launch(string wd, string command = null);
    }
}
