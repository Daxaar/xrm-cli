using Microsoft.Xrm.Sdk;

namespace Octono.Xrm.Tasks
{
    public interface ITaskGateway
    {
        Entity RetrieveWebResource(IOrganizationService service, string name);
    }
}