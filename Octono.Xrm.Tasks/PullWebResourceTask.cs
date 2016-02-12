using System;
using System.IO;
using Microsoft.Xrm.Sdk;
using Octono.Xrm.Tasks.IO;

//TODO: Add support for path format resource names i.e new_/resource.js

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Pulls a WebResource from the Organisation to the specified local file
    /// </summary>
    public class PullWebResourceTask : XrmTask
    {
        private readonly PullWebResourceCommandLine _commandLine;
        private readonly IFileWriter _writer;

        public PullWebResourceTask(PullWebResourceCommandLine commandLine, IFileWriter writer)
        {
            _commandLine = commandLine;
            _writer = writer;
        }

        public override void Execute(IXrmTaskContext context)
        {
            if (ShowHelp(context.Log)) return;

            IOrganizationService service = context.ServiceFactory.Create(_commandLine.ConnectionName);
            var query       = new WebResourceQuery();
            var entity      = query.Retrieve(service,_commandLine.Name);
            var content     = entity.GetAttributeValue<string>("content").FromBase64String();
            var optionset   = entity.GetAttributeValue<OptionSetValue>("webresourcetype");
            var path        = _commandLine.Path;

            if (path.EndsWith(@"\"))
            {
                path += _commandLine.Name + WebResourceType.ToFileExtension(optionset.Value);
            }

            if (File.Exists(path) && _commandLine.Overwrite == false)
            {
                if (context.Log.Prompt("Overwrite existing file? y/n").Contains("n"))
                {
                    context.Log.Write("Pull cancelled");
                    return;
                }
            }
            _writer.Write(System.Text.Encoding.UTF8.GetBytes(content),path);
            context.Log.Write("File written to " + path);
        }

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