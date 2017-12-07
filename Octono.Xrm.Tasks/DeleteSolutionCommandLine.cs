using System.Collections.Generic;
using System.Linq;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Command line argument parser for DeleteSolutionTask
    /// </summary>
    public class DeleteSolutionCommandLine : CommandLine
    {
        public DeleteSolutionCommandLine(IList<string> args) : base(args)
        {
            SolutionName = args[1];
            ShowHelp = args.Any(arg => arg == "--help" || arg == "-help");
        }

        public bool ShowHelp { get; }
        public string SolutionName { get; }
    }
}