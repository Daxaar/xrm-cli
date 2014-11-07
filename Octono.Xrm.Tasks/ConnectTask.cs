using Microsoft.Xrm.Sdk.Client;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Not currently used but will be once REPL features are implemented.
    /// </summary>
    public class ConnectTask : IXrmTask
    {
        public void Execute(IXrmTaskContext context)
        {
            context.Log.Write("Connected to " + ((OrganizationServiceProxy)context.Service).ServiceManagement.CurrentServiceEndpoint.Address);
        }
        public bool RequiresServerConnection { get { return true; } }
    }
}