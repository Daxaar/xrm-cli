using System.Linq;

namespace Octono.Xrm.Tasks
{
    public class PullWebResourceCommandLine
    {
        private readonly string[] _args;
        private bool _showHelp;

        public PullWebResourceCommandLine(string[] args)
        {
            _args = args;
        }

        public string Name { get { return _args[1]; } }
        public string Path { get { return _args[_args.Length-1]; } }

        public bool ShowHelp
        {
            get { return _args.Contains("-help") || _args.Contains("/?") || _args.Contains("--help"); }
        }
    }
}