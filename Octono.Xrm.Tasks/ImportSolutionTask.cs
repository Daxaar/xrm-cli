using System;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Octono.Xrm.Tasks
{
    public class ImportSolutionTask : IXrmTask
    {
        private readonly ImportSolutionCommandLine _command;

        public ImportSolutionTask(ImportSolutionCommandLine command)
        {
            _command = command;
        }

        public void Execute(IXrmTaskContext context)
        {
            if (ShowHelp(context.Log)) return;

            foreach (var filePath in _command.GetSolutionFilePaths())
            {
                context.Log.Write(string.Format("Importing {0}", filePath));
                Guid jobId = Guid.NewGuid();
                var importRequest = new ImportSolutionRequest
                {
                    PublishWorkflows = _command.Publish,
                    CustomizationFile = _command.ReadFile(filePath),
                    ImportJobId = jobId,
                    OverwriteUnmanagedCustomizations = _command.OverwriteUmanaged
                };
                IOrganizationService service = context.ServiceFactory.Create(_command.ConnectionName);
                service.Execute(importRequest);
                
                context.Log.Write(string.Format("Publishing {0}",filePath));
                if (_command.Publish)
                {
                    service.Execute(new PublishAllXmlRequest());
                }
                service.Retrieve("importjob", importRequest.ImportJobId, new ColumnSet(new[] { "data", "solutionname" }));
                context.Log.Write("Solution imported successfully");
                
            }
        }

        private bool ShowHelp(ILog log)
        {
            if (_command.ShowHelp)
            {
                //_log.Write("Usage\n");
                log.Write(@"import ");
                log.Write("\tWithout any args imports all solutions in the Imports folder");
                log.Write(@"import solutionname x:\path\to\solutionname.zip");
                log.Write(@"import solutionname1,solutionname2 x:\path\to\imports\folder");
                log.Write(@"import solutionname1,solutionname2");
                log.Write("\tImports listed solutions in the Imports folder");
                log.Write(@"import solutionname1,solutionname2 --exports");
                log.Write("\tImports listed solutions in the Exports folder");
                log.Write(@"import --exports");
                log.Write("\tImports all solutions in the Exports folder");
                return true;
            }
            return false;
        }
    }
}
