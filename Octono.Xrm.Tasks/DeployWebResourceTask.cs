using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xrm.Sdk;
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
    public class DeployWebResourceTask : XrmTask
    {
        private readonly DeployWebResourceCommandLine _commandLine;
        private readonly IFileReader _reader;

        public DeployWebResourceTask(DeployWebResourceCommandLine commandLine, IFileReader reader)
        {
            _commandLine = commandLine;
            _reader = reader;
        }

        public override void Execute(IXrmTaskContext context)
        {
            var content     = _reader.ReadAllBytes(_commandLine.FilePath);
            string fileNameWithoutExtension = _reader.RemoveFileExtension(Path.GetFileName(_commandLine.FilePath));
            string fileContent64 = Convert.ToBase64String(content);

            if (string.IsNullOrEmpty(fileContent64) && !_commandLine.AllowEmptyFile)
            {
                context.Log.Write("The local file is empty.  Use the -f argument if you wish force the deployment.");
                return;
            }

            //Retrieve the existing web resource based on the filename
            context.Log.Write(string.Format("Retrieving {0}", fileNameWithoutExtension));
            IOrganizationService service = context.ServiceFactory.Create(_commandLine.ConnectionName);
            var query = new WebResourceQuery(service);
            var resource = query.Retrieve(fileNameWithoutExtension);

            if (!string.IsNullOrEmpty(resource.GetAttributeValue<string>("content")) &&
                resource.GetAttributeValue<string>("content").Equals(fileContent64, StringComparison.CurrentCulture))
            {
                context.Log.Write("The server content is the same as the local file content");
                return;
            }
            //Set the content of the webresource to the file content and update
            context.Log.Write(string.Format("Deploying {0}", fileNameWithoutExtension));
            resource["content"] = fileContent64;
            service.Update(resource);

            var publish = new PublishWebResourceTask(resource.Id,_commandLine.ConnectionName);
            publish.Execute(context);
            context.Configuration.AppSettings["lastmodified"] = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

    }

    public class WebResourceQuery
    {
        private readonly IOrganizationService _service;

        public WebResourceQuery(IOrganizationService service)
        {
            if(service == null) throw new ArgumentNullException("service","WebResourceQuery ctor paramter cannot be null");
            _service = service;
        }

        public Entity Retrieve(string name)
        {
            var query = new QueryExpression("webresource") { ColumnSet = new ColumnSet(new[] { "content","webresourcetype" }) };

            query.Criteria.AddCondition("name", ConditionOperator.Equal, name);
            //query.Criteria.AddCondition("iscustomizable",ConditionOperator.Equal,true);

            query.Criteria.AddFilter(LogicalOperator.And).AddCondition("iscustomizable",ConditionOperator.Equal,true);
            var result = _service.RetrieveMultiple(query);

            if (result.Entities.Any() == false)
            {
                throw new InvalidOperationException(string.Format("Cannot find JavaScript web resource {0}", name));                
            } 

            return result.Entities.Single();            
        }
    }
}