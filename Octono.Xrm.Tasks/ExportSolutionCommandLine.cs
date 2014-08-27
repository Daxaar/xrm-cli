using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xrm.Sdk.Client;

namespace Octono.Xrm.Tasks
{
    public class ExportSolutionCommandLine
    {
        private readonly string[] _args;

        public ExportSolutionCommandLine(string[] args)
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
            string path = _args.FirstOrDefault(x => x.Contains("out:") || x.Contains("output:") || x.Contains("to:"));

            if (_args.Length > 2 && _args[2].EndsWith(".zip"))
            {
                //export path is specifying a full path including filename check only one solution specified and return path
                if (SolutionNames.Count() > 1)
                {
                    throw new InvalidOperationException("Cannot include filename in export path when multiple solutions have been specified.");
                }
                return _args[2];
            }

            path = string.IsNullOrEmpty(path) ? @"Export" : path.Substring(path.IndexOf(':')+1);

            if (version != "")
            {
                solutionName += "-";
            }
            return Path.Combine(path, solutionName + version.Replace(".","-") + (Managed ? "_managed.zip" : ".zip"));
        }
    }
}