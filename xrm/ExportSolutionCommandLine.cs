using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xrm
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

        public string BuildExportPath(string solutionName)
        {
            string path = _args.FirstOrDefault(x => x.Contains("out:") || x.Contains("output:") || x.Contains("to:"));

            if (_args[2].EndsWith(".zip"))
            {
                //export path is specifying a full path including filename check only one solution specified and return path
                if (SolutionNames.Count() > 1)
                {
                    throw new InvalidOperationException("Cannot include filename in export path when multiple solutions have been specified.");
                }
                return _args[2];
            }

            if (string.IsNullOrEmpty(path))
            {
                path = @"Export";
            }
            else
            {
                path = path.Substring(path.IndexOf(':')+1);
            }
 
            return Path.Combine(path, solutionName + ".zip");
        }
    }
}