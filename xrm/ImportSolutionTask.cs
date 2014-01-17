using System;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Xrm
{
    public class ImportSolutionTask : IXrmTask
    {
        private readonly ImportSolutionCommandLine _command;
        private readonly IOrganizationService _service;
        private readonly ILog _log;

        public ImportSolutionTask(ImportSolutionCommandLine command, IOrganizationService service, ILog log)
        {
            _command = command;
            _service = service;
            _log = log;
        }

        public void Execute()
        {
            _log.Log(string.Format("Importing {0}", _command.SolutionFilePath));
            Guid jobId = Guid.NewGuid();
            var importRequest = new ImportSolutionRequest
                {
                    PublishWorkflows = _command.Publish,
                    CustomizationFile = _command.SolutionFile,
                    ImportJobId = jobId
                };
            _service.Execute(importRequest);

            Entity job = _service.Retrieve("importjob", importRequest.ImportJobId, new ColumnSet(new[] { "data", "solutionname" }));
            _log.Log("Solution imported successfully");
        }
    }
}
