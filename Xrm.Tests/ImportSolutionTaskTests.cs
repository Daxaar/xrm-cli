using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Octono.Xrm.ConsoleTaskRunner;
using Octono.Xrm.Tasks;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class ImportSolutionTaskTests
    {
        [TestMethod]
        public void ImportsAllSolutionsInExportsFolderWhenExportsCommandOptionSet()
        {
            var reader = new Mock<IFileReader>();
            reader.Setup(x => x.GetSolutionsInExportFolder()).Returns(new[] {"solution1.zip", "solution2.zip"});
            reader.Setup(x => x.FileExists("solution1.zip")).Returns(true);
            reader.Setup(x => x.FileExists("solution2.zip")).Returns(true);
            var service = new Mock<IOrganizationService>();
            var command = new ImportSolutionCommandLine(new string[] {"import", "--exports"},reader.Object);
            var task = new ImportSolutionTask(command, service.Object, new ConsoleLogger());

            task.Execute();

            service.Verify(x=>x.Execute(It.IsAny<ImportSolutionRequest>()),Times.Exactly(2));
            reader.Verify(x=>x.ReadAllBytes("solution1.zip"),Times.Once);
            reader.Verify(x=>x.ReadAllBytes("solution2.zip"),Times.Once);
        }

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
            Assert.AreEqual(filePath,command.GetSolutionFilePaths().Single());
        }

        [TestMethod]
        public void DoesNotAttemptToImportSolutionWhenHelpOptionSpecified()
        {
            var service     = new Mock<IOrganizationService>();
            var commandLine = new ImportSolutionCommandLine(new[] {"import", "--help"}, new Mock<IFileReader>().Object);
            var task        = new ImportSolutionTask(  commandLine, service.Object, new ConsoleLogger());
            
            task.Execute();
            
            service.Verify(x=>x.Execute(It.IsAny<OrganizationRequest>()),Times.Never);
        }
    }
}
