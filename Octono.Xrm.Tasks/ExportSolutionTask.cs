using System;
using System.IO;
using System.Linq;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    public class ExportSolutionTask : IXrmTask
    {
        private readonly ExportSolutionCommandLine _command;
        private readonly IFileWriter _writer;
        private readonly IOrganizationService _service;
        private readonly ILog _log;

        public ExportSolutionTask(ExportSolutionCommandLine command, IFileWriter writer, IOrganizationService service,ILog log)
        {
            _command = command;
            _writer = writer;
            _service = service;
            _log = log;
        }

        public void Execute()
        {
            if (ShowHelp()) return;

            foreach (var solution in _command.SolutionNames)
            {
                string version = GetSolutionVersionNumber(solution);
                String path = _command.BuildExportPath(solution,version);

                if (_command.IncrementVersionBeforeExport)
                {
                    var incrementVersionTask = new IncrementSolutionVersionTask(solution,_service, _log);
                    incrementVersionTask.Execute();
                }

                _log.Write(string.Format("Exporting {0} to {1}", solution, path));
                var response = (ExportSolutionResponse)_service.Execute(new ExportSolutionRequest()
                {
                    SolutionName = solution,
                    Managed = _command.Managed
                    
                });

                Directory.CreateDirectory(Path.GetDirectoryName(path));
                _writer.Write(response.ExportSolutionFile,path );
                _log.Write(string.Format("{0} exported successfully",solution));
            }
        }

        private bool ShowHelp()
        {
            if (_command.ShowHelp)
            {
                _log.Write("Usage");
                _log.Write(@"export solutionname x:\path\to\solutionname.zip");
                _log.Write(@"export solutionname1,solutionname2 x:\path\to\exports\folder");
                _log.Write(@"export solutionname1,solutionname2");
                _log.Write("\tExports listed solutions in the Exports folder");
                _log.Write("Switches");
                _log.Write("-m Managed");
                return true;
            }
            return false;
        }

        private string GetSolutionVersionNumber(string solution)
        {
            using (var context = new OrganizationServiceContext(_service))
            {
                try
                {
                    return context.CreateQuery("solution")
                                  .First(x => x.GetAttributeValue<string>("uniquename") == solution)
                                  .GetAttributeValue<string>("version");
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
    }
}
