using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using Xrm;

namespace Xrm.Tests
{
    [TestClass]
    public class ExportSolutionCommandLine
    {
        public ExportSolutionCommandLine(string[] args, IFileReader reader)
        {
            
        }
    }

    [TestClass]
    public class ExportSolutionTaskTests
    {
        [TestMethod]
        public void WritesExportedFileToDefaultExportsPathWhenNotProvidedInArgs()
        {
            var writer = new Mock<IFileWriter>();
            var service = new Mock<IOrganizationService>();
            var export = new ExportSolutionTask("solutionname", writer.Object, service.Object, String.Empty);

            service.Setup(x => x.Execute(It.IsAny<OrganizationRequest>())).Returns(new ExportSolutionResponse());
            export.Execute();
            writer.Verify(x => x.Write(It.IsAny<byte[]>(), export.ExportPath), Times.Once());
        }

        [TestMethod]
        public void WritesExportedFileToPathProvided()
        {
            var writer = new Mock<IFileWriter>();
            const string exportPath = @"path\to\export\to";
            var service = new Mock<IOrganizationService>();
            service.Setup(x => x.Execute(It.IsAny<ExportSolutionRequest>())).Returns(new ExportSolutionResponse());

            var task = new ExportSolutionTask("", writer.Object, service.Object, exportPath);
            task.Execute();
            writer.Verify(x=>x.Write(It.IsAny<byte[]>(),task.ExportPath));
        }

        [TestMethod]
        public void CallsOrgService()
        {
            var service = new Mock<IOrganizationService>();
            var task = new ExportSolutionTask("", new Mock<IFileWriter>().Object,service.Object, "path");
            task.Execute();
            service.Verify(x=>x.Execute(It.IsAny<ExportSolutionRequest>()),Times.Once());
        }
    }
}
