using Microsoft.Xrm.Sdk.Client;

namespace Octono.Xrm.Tasks
{
    public class ConnectTask : IXrmTask
    {
        public void Execute(IXrmTaskContext context)
        {
            context.Log.Write("Connected to " + ((OrganizationServiceProxy)context.Service).ServiceManagement.CurrentServiceEndpoint.Address);
        }
        public bool RequiresServerConnection { get { return true; } }
    }
}