using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xrm.Tests
{
    public class SolutionImportCommandLine : IXrmCommandLine
    {
        private readonly IFileReader _reader = null;

        public SolutionImportCommandLine(IFileReader reader)
        {
            Publish = true;
            ActivateProcesses = true;
            _reader = reader;
        }

        public void Parse(string command)
        {
            var args = command.Split(' ');
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
