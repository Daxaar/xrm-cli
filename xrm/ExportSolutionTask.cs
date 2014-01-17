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
        private readonly string _solutionname;
        private readonly IFileWriter _writer;
        private readonly IOrganizationService _service;
        private readonly string _exportPath;

        public ExportSolutionTask(string solutionname, IFileWriter writer, IOrganizationService service, string exportPath)
        {
            _solutionname = solutionname;
            _writer = writer;
            _service = service;
            _exportPath = exportPath;
        }

        public void Execute()
        {
            var response = (ExportSolutionResponse)_service.Execute(new ExportSolutionRequest()
                {
                    SolutionName = _solutionname,
                });

            if (response != null)
            {
                _writer.Write(response.ExportSolutionFile, ExportPath );                
            }
        }

        public string ExportPath
        {
            get
            {
                return Path.Combine(string.IsNullOrEmpty(_exportPath) ? AppDomain.CurrentDomain.BaseDirectory : _exportPath, "Export", _solutionname + ".zip");
            }
        }
    }
}
