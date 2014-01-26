using System;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Octono.Xrm.Tasks
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
            if (ShowHelp()) return;

            foreach (var filePath in _command.GetSolutionFilePaths())
            {
                _log.Write(string.Format("Importing {0}", filePath));
                Guid jobId = Guid.NewGuid();
                var importRequest = new ImportSolutionRequest
                {
                    PublishWorkflows = _command.Publish,
                    CustomizationFile = _command.ReadFile(filePath),
                    ImportJobId = jobId
                };
                _service.Execute(importRequest);
                
                _log.Write(string.Format("Publishing {0}",filePath));
                _service.Execute(new PublishAllXmlRequest());

                Entity job = _service.Retrieve("importjob", importRequest.ImportJobId, new ColumnSet(new[] { "data", "solutionname" }));
                _log.Write("Solution imported successfully");
                
            }
        }

        private bool ShowHelp()
        {
            if (_command.ShowHelp)
            {
                //_log.Write("Usage\n");
                _log.Write(@"import ");
                _log.Write("\tWithout any args imports all solutions in the Imports folder");
                _log.Write(@"import solutionname x:\path\to\solutionname.zip");
                _log.Write(@"import solutionname1,solutionname2 x:\path\to\imports\folder");
                _log.Write(@"import solutionname1,solutionname2");
                _log.Write("\tImports listed solutions in the Imports folder");
                _log.Write(@"import solutionname1,solutionname2 --exports");
                _log.Write("\tImports listed solutions in the Exports folder");
                _log.Write(@"import --exports");
                _log.Write("\tImports all solutions in the Exports folder");
                return true;
            }
            return false;
        }
    }
}
