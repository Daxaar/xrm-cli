using System.Linq;

namespace Xrm
{
    public class ImportSolutionCommandLine
    {
        private readonly IFileReader _reader = null;

        public ImportSolutionCommandLine(string[] args, IFileReader reader)
        {
            Publish = true;
            ActivateProcesses = true;
            _reader = reader;
            Parse(args);
        }

        private void Parse(string[] args)
        {
            SolutionFilePath = args[1];
            Publish = !args.Contains("--nopublish");
            ActivateProcesses = !args.Contains("--noactivate");
        }

        public string SolutionFilePath { get; set; }
        public byte[] SolutionFile
        {
            get
            {
                return _reader.ReadAllBytes(SolutionFilePath);
            }
        }
        public bool Publish { get; set; }
        public bool ActivateProcesses { get; set; }
    }
}
