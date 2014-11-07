using Microsoft.Crm.Sdk.Messages;

namespace Octono.Xrm.Tasks
{
    public class PublishSolutionTask : XrmTask
    {
        public override void Execute(IXrmTaskContext context)
        {   
            context.Log.Write("Publishing all changes");
            context.Service.Execute(new PublishAllXmlRequest());
            context.Log.Write("Publish complete");
        }
    }
}
