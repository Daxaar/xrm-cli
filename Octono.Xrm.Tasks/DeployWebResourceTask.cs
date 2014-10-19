using System;
using System.IO;
using System.Linq;
using Microsoft.Xrm.Sdk.Query;
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

        public DeployWebResourceTask(DeployWebResourceCommandLine commandLine, IFileReader reader)
        {
            _commandLine = commandLine;
            _reader = reader;
            RequiresServerConnection = true;
        }

        public void Execute(IXrmTaskContext context)
        {
            var content     = _reader.ReadAllBytes(_commandLine.FilePath);
            string filename = Path.GetFileName(_commandLine.FilePath);
            string fileNameWithoutExtension = _reader.RemoveFileExtension(filename);
            string fileContent64 = Convert.ToBase64String(content);

            if (string.IsNullOrEmpty(fileContent64) && !_commandLine.AllowEmptyFile)
            {
                context.Log.Write("The local file is empty.  Use the -f argument if you wish to proceed");
                context.Log.Write("Deploy aborted");
                return;
            }

            //Retrieve the existing web resource based on the filename
            context.Log.Write(string.Format("Retrieving {0}", fileNameWithoutExtension));
            var query = new QueryExpression("webresource") {ColumnSet = new ColumnSet(new[] {"content"})};

            query.Criteria.AddCondition("id", ConditionOperator.Equal, fileNameWithoutExtension);
            var resource = context.Service.RetrieveMultiple(query).Entities.SingleOrDefault();

            if (resource == null)
                throw new InvalidOperationException(string.Format("Cannot find JavaScript web resource {0}", filename));

            if (!string.IsNullOrEmpty(resource.GetAttributeValue<string>("content")) &&
                resource.GetAttributeValue<string>("content").Equals(fileContent64, StringComparison.CurrentCulture))
            {
                context.Log.Write("The server content is the same as the local file content");
                context.Log.Write("Deploy aborted");
                return;
            }
            //Set the content of the webresource to the file content and update
            context.Log.Write(string.Format("Updating {0}", fileNameWithoutExtension));
            resource["content"] = fileContent64;
            context.Service.Update(resource);

            var publish = new PublishWebResourceTask(resource.Id);
            publish.Execute(context);
        }

        public bool RequiresServerConnection { get; private set; }
    }
}