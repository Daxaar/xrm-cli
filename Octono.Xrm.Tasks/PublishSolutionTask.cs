using Microsoft.Crm.Sdk.Messages;

namespace Octono.Xrm.Tasks
{
    public class PublishSolutionTask : IXrmTask
    {
        public void Execute(IXrmTaskContext context)
        {   
            context.Log.Write("Publishing all changes");
            context.Service.Execute(new PublishAllXmlRequest());
            context.Log.Write("Publish complete");
        }
        public bool RequiresServerConnection { get { return true; } }
    }
}
