using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;

namespace Octono.Xrm.Tasks
{
    public class PublishSolutionCommandLine : CommandLine
    {
        public PublishSolutionCommandLine(IList<string> args) : base(args)
        {
        }
    }
    public class PublishSolutionTask : XrmTask
    {
        private readonly PublishSolutionCommandLine _commandLine;

        public PublishSolutionTask(PublishSolutionCommandLine commandLine)
        {
            _commandLine = commandLine;
        }

        public override void Execute(IXrmTaskContext context)
        {   
            context.Log.Write("Publishing all changes");
            context.ServiceFactory.Create(_commandLine.ConnectionName).Execute(new PublishAllXmlRequest());
            context.Log.Write("Publish complete");
        }
    }
}
