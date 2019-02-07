using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloop.GitHub.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var ghs = new GitHubPlugin();
            var repo = ghs.Query(null);
        }
    }
}
