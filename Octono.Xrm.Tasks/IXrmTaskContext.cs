using Microsoft.Xrm.Sdk;

namespace Octono.Xrm.Tasks
{
    public interface IXrmTaskContext
    {
        IOrganizationService Service { get; }
        ILog Log { get; }
    }
}
