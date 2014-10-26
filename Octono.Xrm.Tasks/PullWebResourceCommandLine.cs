namespace Octono.Xrm.Tasks
{
    public class PullWebResourceCommandLine
    {
        private readonly string[] _args;

        public PullWebResourceCommandLine(string[] args)
        {
            _args = args;
        }

        public string Name { get { return _args[1]; } }
        public string Path { get { return _args[_args.Length-1]; } }
    }
}