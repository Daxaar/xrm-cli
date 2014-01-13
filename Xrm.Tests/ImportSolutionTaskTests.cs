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
            var command = new ImportSolutionCommandLine(new[] {"import", @"c:\path\to\file.zip"}, reader.Object);
            IXrmTask task = new ImportSolutionTask(command, service.Object);
            
            reader.Setup(x => x.ReadAllBytes(command.SolutionFilePath)).Returns(new byte[] { });
            task.Execute();
            service.Verify(x => x.Execute(It.IsAny<ImportSolutionRequest>()),Times.Once());
            reader.Verify(x => x.ReadAllBytes(command.SolutionFilePath), Times.Once());
        }
    }
}
