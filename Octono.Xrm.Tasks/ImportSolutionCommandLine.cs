using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    public class ImportSolutionCommandLine : CommandLine
    {
        private readonly IList<string> _args;
        private readonly IFileReader _reader;

        public ImportSolutionCommandLine(IList<string> args, IFileReader reader) : base(args)
        {
            _args = args;
            _reader = reader;
        }

        public IEnumerable<string> GetSolutionFilePaths()
        {
                //Read the import filename (inc path) from second commandline arg or read all solution files
                //from Export directory if --exports option has been specified
                string[] files = _args.Contains("--exports") 
                                    ? _reader.GetSolutionsInExportFolder().Where(x => x.Contains(":") == false).ToArray() 
                                    : _args[1].Split(',');

                for (int i = 0; i < files.Length; i++)
                {
                    if (!files[i].EndsWith(".zip"))
                    {
                        files[i] += ".zip";
                    }
                }

                foreach (var file in files)
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    if (_reader.FileExists(file))
                    {
                        yield return file;
                    }
                    else if (_reader.FileExists(Path.Combine(baseDirectory, "Import", file)))
                    {
                        yield return Path.Combine(baseDirectory, "Import", file);
                    }
                    else if (_reader.FileExists(Path.Combine(baseDirectory, file)))
                    {
                        yield return Path.Combine(baseDirectory, file);
                    }
                    else
                    {
                        throw new Exception($"Cannot find file {file}.  If you have specified multiple files they must be comma separated");                                            
                    }
                }
        }
        

        public byte[] ReadFile(string solutionFilePath)
        {
            return _reader.ReadAllBytes(solutionFilePath);
        }

        public IEnumerable<byte[]> SolutionFiles
        {
            get 
            {
                return GetSolutionFilePaths().Select(file => _reader.ReadAllBytes(file));
            }
        }
        public bool Publish => !_args.Contains("--nopublish");
        public bool ActivateProcesses => !_args.Contains("--noactivate");

        public bool ShowHelp => _args.Contains("--help");

        public bool OverwriteUmanaged => _args.Contains("--overwrite") || _args.Contains("-o") || _args.Contains("--o");
    }
}
