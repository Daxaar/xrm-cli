using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class ImportSolutionTaskTests
    {
        [TestMethod]
        public void ImportsAllSolutionsInExportsFolderWhenExportsCommandOptionSet()
        {
            var reader  = new Mock<IFileReader>();
            var context = new Mock<IXrmTaskContext>();
            var service = new Mock<IOrganizationService>();
            
            context.Setup(x => x.ServiceFactory.Create(It.IsAny<string>())).Returns(service.Object);
            context.Setup(x => x.Log).Returns(new Mock<ILog>().Object);

            reader.Setup(x => x.GetSolutionsInExportFolder()).Returns(new[] { "solution1.zip", "solution2.zip", "conn:connectionName" });
            reader.Setup(x => x.FileExists("solution1.zip")).Returns(true);
            reader.Setup(x => x.FileExists("solution2.zip")).Returns(true);
            var command = new ImportSolutionCommandLine(new[] { "import", "--exports", "conn:connectionName" }, reader.Object);
            var task = new ImportSolutionTask(command);

            task.Execute(context.Object);

            service.Verify(x=>x.Execute(It.IsAny<ImportSolutionRequest>()),Times.Exactly(2));
            reader.Verify(x=>x.ReadAllBytes("solution1.zip"),Times.Once);
            reader.Verify(x=>x.ReadAllBytes("solution2.zip"),Times.Once);
        }

        [TestMethod]
        public void ExecutesImportRequestWhenCommandIsValid()
        {
            var service = new Mock<IOrganizationService>();
            var reader = new Mock<IFileReader>();
            var context = new Mock<IXrmTaskContext>();
            context.Setup(x => x.ServiceFactory.Create(It.IsAny<string>())).Returns(service.Object);
            context.Setup(x => x.Log).Returns(new Mock<ILog>().Object);

            const string filePath = @"c:\path\to\file.zip";

            var command = new ImportSolutionCommandLine(new[] { "import", filePath, "conn:connectionName" }, reader.Object);
            
            reader.Setup(x => x.ReadAllBytes(filePath)).Returns(new byte[] { });
            reader.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

            IXrmTask task = new ImportSolutionTask(command);
            task.Execute(context.Object);

            service.Verify(x => x.Execute(It.IsAny<ImportSolutionRequest>()),Times.Once());
            Assert.AreEqual(filePath,command.GetSolutionFilePaths().Single());
        }

        [TestMethod]
        public void DoesNotAttemptToImportSolutionWhenHelpOptionSpecified()
        {
            var service = new Mock<IOrganizationService>();
            var context = new Mock<IXrmTaskContext>();
            context.Setup(x => x.ServiceFactory.Create(It.IsAny<string>())).Returns(service.Object);
            context.Setup(x => x.Log).Returns(new Mock<ILog>().Object);
            var commandLine = new ImportSolutionCommandLine(new[] { "import", "--help", "conn:connectionName" }, new Mock<IFileReader>().Object);
            var task        = new ImportSolutionTask(commandLine);
            
            task.Execute(context.Object);
            
            service.Verify(x=>x.Execute(It.IsAny<OrganizationRequest>()),Times.Never);
        }

        [TestMethod]
        public void ImportsOnlyTheFileSpecifiedWhenReadingFromExportsFolder()
        {
            //TODO:  Noticed a bug whilst using tool that started reading all solutions rather than just the one specified
        }
    }
}
