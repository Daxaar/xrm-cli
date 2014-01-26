using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Octono.Xrm.Tasks
{
    public class ConnectTask : IXrmTask
    {
        private readonly IOrganizationService _service;
        private readonly ILog _logger;

        public ConnectTask(IOrganizationService service, ILog logger)
        {
            _service = service;
            _logger = logger;
        }

        public void Execute()
        {
            _logger.Write("Connected to " + ((OrganizationServiceProxy)_service).ServiceManagement.CurrentServiceEndpoint.Address);
        }
    }
}