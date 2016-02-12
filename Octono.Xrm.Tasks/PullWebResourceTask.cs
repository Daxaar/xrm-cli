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
        private readonly WebResourceMetaData _metaData;

        public PullWebResourceTask(PullWebResourceCommandLine commandLine, IFileWriter writer, WebResourceMetaData metaData)
        {
            _commandLine = commandLine;
            _writer = writer;
            _metaData = metaData;
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

            //Write the original webresource metadata to a file for deploy task
            _metaData.Create(_commandLine.Name,_commandLine.Path);
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
}