using System;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.Xrm.Sdk;
using Octono.Xrm.Tasks.IO;
//TODO: Adding warning when updating a managed webresource
//TODO: Support other web resource types
//TODO: Add metadata support and creation when doesn't exist

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Updates an existing webresource (only JavaScript for now) from the filesystem and publishes the change. 
    /// </summary>
    /// <remarks>
    /// Currently assumes the webresource exists on the server and therefore only updates the content.
    /// </remarks>
    public class DeployWebResourceTask : IXrmTask
    {
        private readonly DeployWebResourceCommandLine _commandLine;
        private readonly IFileReader _reader;
        private readonly IWebResourceQuery _query;
        private readonly IWebResourceMetaData _metadata;

        public DeployWebResourceTask(DeployWebResourceCommandLine commandLine, IFileReader reader, IWebResourceQuery query, IWebResourceMetaData metadata)
        {
            _commandLine = commandLine;
            _reader = reader;
            _query = query;
            _metadata = metadata;
        }

        public void Execute(IXrmTaskContext context)
        {
            var content = _reader.ReadAllBytes(_commandLine.FilePath);
            string fileContent64 = Convert.ToBase64String(content);

            if (!ContinueWhenFileIsEmpty(context,fileContent64)) return;

            //Use the name if specified on the commandline otherwise default to filename without file extension
            var metadata = _metadata.Load(_commandLine.FilePath);
            
            string resourceName = _commandLine.Name ?? metadata.WebResourceName ?? _reader.RemoveFileExtension(Path.GetFileName(_commandLine.FilePath));

            //Retrieve the existing web resource
            context.Log.Write(string.Format("Retrieving {0}", resourceName));
            IOrganizationService service = context.ServiceFactory.Create(_commandLine.ConnectionName);
            var resource = _query.Retrieve(service, resourceName);

            if (FileOnServerMatchesLocalFile(resource,context,fileContent64)) return;

            //Set the content of the webresource to the file content and update
            context.Log.Write(string.Format("Deploying {0}", resourceName));
            resource["content"] = fileContent64;
            service.Update(resource);

            var publish = new PublishWebResourceTask(resource.Id,_commandLine.ConnectionName);
            publish.Execute(context);
            context.Configuration.AppSettings["lastmodified"] = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        private static bool FileOnServerMatchesLocalFile(Entity resource, IXrmTaskContext context, string fileContent64)
        {
            if (!string.IsNullOrEmpty(resource.GetAttributeValue<string>("content")) &&
                resource.GetAttributeValue<string>("content").Equals(fileContent64, StringComparison.CurrentCulture))
            {
                context.Log.Write("The server content is the same as the local file content");
                return true;
            }
            return false;
        }

        private bool ContinueWhenFileIsEmpty(IXrmTaskContext context, string fileContent64)
        {
            if (string.IsNullOrEmpty(fileContent64) && !_commandLine.AllowEmptyFile)
            {
                context.Log.Write("The local file is empty.  Use the -f argument if you wish force the deployment.");
                return false;
            }
            return true;
        }
    }
}