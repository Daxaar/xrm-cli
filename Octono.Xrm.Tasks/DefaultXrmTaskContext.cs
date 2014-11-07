using Microsoft.Xrm.Sdk;

namespace Octono.Xrm.Tasks
{
    internal class DefaultXrmTaskContext : IXrmTaskContext
    {
        public DefaultXrmTaskContext(IOrganizationService service, ILog log)
        {
            Service = service;
            Log = log;
        }
        public DefaultXrmTaskContext(ILog log)
        {
            Log = log;
        }

        public IOrganizationService Service { get; private set; }
        public ILog Log { get; private set; }
    }
}
