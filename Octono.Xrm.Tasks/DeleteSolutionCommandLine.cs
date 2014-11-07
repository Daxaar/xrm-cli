using System.Collections.Generic;
using System.Linq;

namespace Octono.Xrm.Tasks
{
    public class DeleteSolutionCommandLine
    {
        public DeleteSolutionCommandLine(IList<string> args)
        {
            SolutionName = args[1];
            ShowHelp = args.Any(arg => arg == "--help" || arg == "-help");
        }

        public bool ShowHelp { get; private set; }
        public string SolutionName { get; private set; }
    }
}