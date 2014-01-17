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
            string path = _args.FirstOrDefault(x => x.Contains("out:") || x.Contains("output:"));

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