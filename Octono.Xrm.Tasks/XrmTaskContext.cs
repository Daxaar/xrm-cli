using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Passed to all tasks allowing access to contextual information such as CRM Organization Service Connections
    /// </summary>
    public class XrmTaskContext : IXrmTaskContext
    {

        public XrmTaskContext(IXrmServiceFactory factory, ILog log, IXrmConfiguration configuration)
        {
            ServiceFactory = factory;
            Log = log;
            Configuration = configuration;
        }

        public ILog Log { get; }
        public IXrmServiceFactory ServiceFactory { get; }
        public IXrmConfiguration Configuration { get; }
    }
}
