using System;
using Microsoft.Crm.Sdk.Messages;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Publishes a WebResource typically after depoyment using DeployWebResourceTask
    /// </summary>
    public class PublishWebResourceTask : IXrmTask
    {
        private readonly Guid _id;
        private readonly string _connectionName;

        public PublishWebResourceTask(Guid id, string connectionName)
        {
            _id = id;
            _connectionName = connectionName;
        }

        public void Execute(IXrmTaskContext context)
        {
            //Publish the change
            context.Log.Write("Publishing");
            var publish = new PublishXmlRequest();
            string webResourceXml =
                string.Format(
                    "<importexportxml><webresources><webresource>{0}</webresource></webresources></importexportxml>",
                    _id);

            publish.ParameterXml = webResourceXml;
            var service = context.ServiceFactory.Create(_connectionName);

            try
            {
                service.Execute(publish);
            }
            catch (Exception ex)
            {
                context.Log.Write("An error occurred publish the Web Resource.");
                context.Log.Write(ex.Message);
               throw;
            }
        }
    }
}