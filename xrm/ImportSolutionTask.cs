using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public class ImportSolutionTask : IXrmTask
    {
        private readonly ImportSolutionCommandLine _command;
        private readonly IOrganizationService _service;

        public ImportSolutionTask(ImportSolutionCommandLine command, IOrganizationService service)
        {
            _command = command;
            _service = service;
        }

        public void Execute()
        {
            _service.Execute(new ImportSolutionRequest() {
                PublishWorkflows = _command.Publish,
                CustomizationFile = _command.SolutionFile,
            });
        }
    }
}
