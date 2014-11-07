using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Octono.Xrm.Tasks
{
    public class PullWebResourceCommandLine
    {
        private readonly List<string> _args;
        private readonly string _path = "";

        public PullWebResourceCommandLine(IEnumerable<string> args)
        {
            _args = args.ToList();

            //If the user has explicitly defined the path anywhere in the args using the to: prefix
            //If not get the first arg that is a valid directory, failing that assume current directory
            _path = _args.Find(x => x.StartsWith("to:")) ??
                    _args.Skip(2).FirstOrDefault(x => !x.StartsWith("-") && Directory.Exists(System.IO.Path.GetDirectoryName(x) ?? "")) ?? 
                    Directory.GetCurrentDirectory();
        }

        public string Name { get { return _args[1]; } }
        public string Path { get{return _path; } }

        public bool ShowHelp
        {
            get { return _args.Contains("-help") || _args.Contains("/?") || _args.Contains("--help"); }
        }

        public bool Overwrite { get { return _args.Contains("--overwrite") || _args.Contains("-o"); } }
    }
}