using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    public interface IXrmTaskContext
    {
        ILog Log { get; }
        IXrmServiceFactory ServiceFactory { get; }
        IXrmConfiguration Configuration { get; }
    }
}