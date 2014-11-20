using Microsoft.Xrm.Sdk;

namespace Octono.Xrm.Tasks
{
    public interface IXrmServiceFactory
    {
        IOrganizationService Create(string connectionName);
    }
}