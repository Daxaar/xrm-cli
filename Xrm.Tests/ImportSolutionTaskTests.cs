using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;

namespace Xrm.Tests
{
    [TestClass]
    public class ImportSolutionTaskTests
    {
        [TestMethod]
        public void ExecutesImportRequestWhenCommandIsValid()
        {
            var service = new Mock<IOrganizationService>();
            var reader = new Mock<IFileReader>();
            const string filePath = @"c:\path\to\file.zip";

            var command = new ImportSolutionCommandLine(new[] { "import", filePath }, reader.Object);
            
            reader.Setup(x => x.ReadAllBytes(filePath)).Returns(new byte[] { });
            reader.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

            IXrmTask task = new ImportSolutionTask(command, service.Object, new Mock<ILog>().Object);
            task.Execute();

            service.Verify(x => x.Execute(It.IsAny<ImportSolutionRequest>()),Times.Once());
            Assert.AreEqual(filePath,command.SolutionFilePath);
        }
    }
}
