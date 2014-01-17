using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public class ExportSolutionTask : IXrmTask
    {
        private readonly ExportSolutionCommandLine _command;
        private readonly IFileWriter _writer;
        private readonly IOrganizationService _service;
        private readonly ILog _log;
        private readonly string _exportPath;

        public ExportSolutionTask(ExportSolutionCommandLine command, IFileWriter writer, IOrganizationService service,ILog log)
        {
            _command = command;
            _writer = writer;
            _service = service;
            _log = log;
        }

        public void Execute()
        {
            foreach (var solution in _command.SolutionNames)
            {
                String path = _command.BuildExportPath(solution);
                _log.Write(string.Format("Exporting {0} to {1}", solution, path));

                var response = (ExportSolutionResponse)_service.Execute(new ExportSolutionRequest()
                {
                    SolutionName = solution,
                });

                Directory.CreateDirectory(Path.GetDirectoryName(path));
                _writer.Write(response.ExportSolutionFile,path );
                _log.Write(string.Format("{0} exported successfully",solution));
            }
        }
    }
}
