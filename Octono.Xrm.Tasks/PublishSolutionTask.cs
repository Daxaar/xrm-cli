using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Executes a PublishAllRequests against the Organisation
    /// </summary>
    public class PublishSolutionCommandLine : CommandLine
    {
        public PublishSolutionCommandLine(IList<string> args) : base(args)
        {
        }
    }
    public class PublishSolutionTask : IXrmTask
    {
        private readonly PublishSolutionCommandLine _commandLine;

        public PublishSolutionTask(PublishSolutionCommandLine commandLine)
        {
            _commandLine = commandLine;
        }

        public void Execute(IXrmTaskContext context)
        {   
            context.Log.Write("Publishing all changes");
            context.ServiceFactory.Create(_commandLine.ConnectionName).Execute(new PublishAllXmlRequest());
            context.Log.Write("Publish complete");
        }
    }
}
