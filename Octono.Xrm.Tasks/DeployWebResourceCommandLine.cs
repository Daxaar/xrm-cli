using System;
using System.Collections.Generic;
using System.Linq;
using Octono.Xrm.Tasks.Utils;

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
        public string FilePath => Args[1];

        public bool AllowEmptyFile => Args.Contains("-f") || Args.Contains("-force");

        public DateTime? LastModified
        {
            get
            { 
                var modified = Args.FirstOrDefault(arg => arg.StartsWith("m:"));
                if (modified == null) return null;
                if (DateTime.TryParse(modified.Substring(2), out var lastModified))
                {
                    return lastModified;
                }
                return null;
            }
        }

        /// <summary>
        /// When an existing filename doesn't match the WebResource name this param allows the
        /// name to be specified
        /// </summary>
        /// <remarks>
        /// Example:
        /// Deploy the file myresource.js overwriting the webresource on the server with the name new_myresource
        /// Note that this won't create a web resource that doesn't already exist
        /// xrm deploy "c:\path\to\myresource.js" name:new_myresource
        /// </remarks>
        public string Name
        {
            get
            {
                var name = Args.FirstOrDefault(arg => arg.StartsWith("name:"));
                return name != null ? InvalidFileName.Unescape(name.Substring(5)) : null;
            }
        }

        public bool Confirm { get { return !Args.Contains("-nc") && !Args.Contains("--noconfirm"); } }
    }
}