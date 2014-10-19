using System;
using Microsoft.Crm.Sdk.Messages;

namespace Octono.Xrm.Tasks
{
    public class PublishWebResourceTask : IXrmTask
    {
        private readonly Guid _id;

        public PublishWebResourceTask(Guid id)
        {
            _id = id;
            RequiresServerConnection = true;
        }

        public void Execute(IXrmTaskContext context)
        {
            //Publish the change
            context.Log.Write(string.Format("Publishing {0}", _id));
            var publish = new PublishXmlRequest();
            string webResourceXml = string.Format("<importexportxml><webresources><webresource>{0}</webresource></webresources></importexportxml>", _id);

            publish.ParameterXml = webResourceXml;
            context.Service.Execute(publish);

        }

        public bool RequiresServerConnection { get; private set; }
    }
}