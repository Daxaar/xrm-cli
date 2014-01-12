using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xrm.Tests
{
    public class SolutionImportTask : IXrmTask
    {
        private readonly SolutionImportCommandLine _command;
        private readonly IOrganizationService _service;

        public SolutionImportTask(SolutionImportCommandLine command, IOrganizationService service)
        {
            _command = command;
            _service = service;
        }

        public void Execute()
        {
            _service.Execute(new ImportSolutionRequest() {
                PublishWorkflows = _command.Publish,
                CustomizationFile = _command.SolutionFile
            });
        }
    }
}
