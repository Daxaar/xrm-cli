using Microsoft.Xrm.Sdk;

namespace Octono.Xrm.Tasks
{
    public interface IXrmTaskContext
    {
        IOrganizationService Service { get; }
        ILog Log { get; }
    }

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

    public interface IXrmTask
    {
        void Execute(IXrmTaskContext context);
        bool RequiresServerConnection { get; }
    }
}
