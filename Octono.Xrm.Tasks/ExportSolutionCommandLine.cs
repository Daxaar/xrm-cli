using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Command line argument parser for ExportSolutionTask
    /// </summary>
    public class ExportSolutionCommandLine : CommandLine
    {
        private readonly IList<string> _args;

        public ExportSolutionCommandLine(IList<string> args) : base(args)
        {
            _args = args;
        }

        public IEnumerable<string> SolutionNames { get
        {
            return _args[1].Split(',');
        }}

        public bool ShowHelp { get { return _args.Contains("--help"); } }

        public bool Managed { get { return _args.Contains("-m"); } }

        public bool IncrementVersionBeforeExport
        {
            get { return _args.Contains("-i"); }
        }

        public string BuildExportPath(string solutionName, string version = "")
        {
            //export    SolutionName    Path    ConnectionName
            //0         1               2       3

            if (_args[2].EndsWith(".zip"))
            {
                //export path is specifying a full path including filename check only one solution specified and return path
                if (SolutionNames.Count() > 1)
                {
                    throw new InvalidOperationException("Cannot include filename in export path when multiple solutions have been specified.");
                }
                return _args[2];
            }

            //If path has been specified use it otherwise use Export directory in current folder
            string path = "Export";
            if (_args.Count != 3 && _args[2].StartsWith("-") == false)
            {
                path = _args[2];
            }

            if (version != "")
            {
                solutionName += "-";
            }
            return Path.Combine(path, solutionName + version.Replace(".","-") + (Managed ? "_managed.zip" : ".zip"));
        }
    }
}