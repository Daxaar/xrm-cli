using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace Octono.Xrm.Tasks
{
    public class PublishSolutionTask : IXrmTask
    {
        private readonly IOrganizationService _service;
        private readonly ILog _logger;

        public PublishSolutionTask(IOrganizationService service,ILog logger)
        {
            _service = service;
            _logger = logger;
        }

        public void Execute()
        {   
            _logger.Write("Publishing all changes");
            _service.Execute(new PublishAllXmlRequest());
            _logger.Write("Publish complete");
        }
    }
}
