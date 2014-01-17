using System;
using System.IO;
using System.Linq;

namespace Xrm
{
    public class ImportSolutionCommandLine
    {
        private readonly string[] _args;
        private readonly IFileReader _reader = null;

        public ImportSolutionCommandLine(string[] args, IFileReader reader)
        {
            _args = args;
            _reader = reader;
        }

        public string SolutionFilePath
        {
            get
            {
                var files = _args[1].Split(',');

                for (int i = 0; i < files.Length; i++)
                {
                    if (!files[1].EndsWith(".zip"))
                    {
                        files[i] += ".zip";
                    }
                }

                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                if (_reader.FileExists(_args[1]))
                {
                    return _args[1];
                }
                if (_reader.FileExists(Path.Combine(baseDirectory, "Import", _args[1])))
                {
                    return Path.Combine(baseDirectory, "Import", _args[1]);
                }
                if (_reader.FileExists(Path.Combine(baseDirectory, _args[1])))
                {
                    return Path.Combine(baseDirectory, _args[1]);
                }
                throw new Exception(string.Format("Cannot find file {0}", _args[1]));
            }
        }

        public byte[] SolutionFile
        {
            get
            {
                return _reader.ReadAllBytes(SolutionFilePath);
            }
        }
        public bool Publish { get { return !_args.Contains("--nopublish"); } }
        public bool ActivateProcesses { get { return !_args.Contains("--noactivate"); } }
    }
}
