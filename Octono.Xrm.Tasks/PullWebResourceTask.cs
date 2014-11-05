using System;
using System.IO;
using Microsoft.Xrm.Sdk;
using Octono.Xrm.Tasks.IO;

//TODO: Add support for retrieving all customizable resources
//TODO: Add support for path format resource names i.e new_/resource.js

namespace Octono.Xrm.Tasks
{
    public class PullWebResourceTask : IXrmTask
    {
        private readonly PullWebResourceCommandLine _commandLine;
        private readonly IFileWriter _writer;

        public PullWebResourceTask(PullWebResourceCommandLine commandLine, IFileWriter writer)
        {
            _commandLine = commandLine;
            _writer = writer;
            RequiresServerConnection = true;
        }

        public void Execute(IXrmTaskContext context)
        {
            if (ShowHelp(context.Log)) return;

            var query       = new WebResourceQuery(context.Service);
            var entity      = query.Retrieve(_commandLine.Name);
            var content     = entity.GetAttributeValue<string>("content").FromBase64String();
            var optionset   = entity.GetAttributeValue<OptionSetValue>("webresourcetype");
            var type        = WebResourceType.ToFileExtension(optionset.Value);
            var path        = _commandLine.Path;
            
            if(string.IsNullOrEmpty(path))
                path = Directory.GetCurrentDirectory();

            var filePath    = Path.Combine(path, _commandLine.Name + type );

            if (File.Exists(filePath) && _commandLine.Overwrite == false)
            {
                if (context.Log.Prompt("Overwrite existing file? y/n").Contains("n"))
                {
                    context.Log.Write("Pull cancelled");
                    return;
                }
            }
            _writer.Write(System.Text.Encoding.UTF8.GetBytes(content),filePath);
            context.Log.Write("File written to " + filePath);
        }

        public bool RequiresServerConnection { get; private set; }

        private bool ShowHelp(ILog log)
        {
            if (_commandLine.ShowHelp)
            {
                log.Write("Usage");
                log.Write(@"pull webresourcelogicalname x:\path\to\save\to\filename.xyz");
                log.Write(@"Optionally omit the path to save the file to the current directory");
                log.Write(@"Server Connection params are required if they haven't already been saved to config");
                return true;
            }
            return false;
        }
    }

    public static class WebResourceType
    {
        public static string ToFileExtension(int value)
        {
            switch (value)
            {
                case 1:
                    return ".htm";
                case 2:
                    return ".css";
                case 3:
                    return ".js";
                case 4:
                    return ".xml";
                case 5:
                    return ".png";
                case 6:
                    return ".jpg";
                case 7:
                    return ".gif";
                case 8:
                    return ".xap";
                case 9:
                    return ".xsl";
                case 10:
                    return ".ico";
                default:
                    throw new ArgumentOutOfRangeException(string.Format("The web resource type with value {0} is unknown.", value));
            }
        }
    }
}