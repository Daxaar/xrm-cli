using System;
using System.Collections.Generic;
using System.Linq;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Command line parser for DeployWebResourceTask
    /// </summary>
    public class DeployWebResourceCommandLine : CommandLine
    {
        public DeployWebResourceCommandLine(IList<string> args) : base(args)
        {
        }

        //TODO: fix this!  It's hacky needing to create an arg string for the base on child tasks
        public DeployWebResourceCommandLine(string fileName, string connectionName) : base(new[]{"deploy", fileName,connectionName})
        {
        }
        public string FilePath { get { return Args[1]; } }

        public bool AllowEmptyFile
        {
            get { return Args.Contains("-f") || Args.Contains("-force"); }
        }

        public DateTime? LastModified
        {
            get
            { 
                var modified = Args.FirstOrDefault(arg => arg.StartsWith("m:"));
                if (modified != null)
                {
                    DateTime lastModified;
                    if (DateTime.TryParse(modified.Substring(2), out lastModified))
                    {
                        return lastModified;
                    }
                }
                return null;
            }
        }

        public bool Confirm { get { return !Args.Contains("-nc") && !Args.Contains("--noconfirm"); } }
    }
}