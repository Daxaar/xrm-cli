using System.Linq;

using Moq;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;
using Octono.Xrm.Tests.Builders;
using Xunit;


namespace Octono.Xrm.Tests
{
    
    public class ImportSolutionTaskTests
    {
        [Fact]
        public void ImportsAllSolutionsInExportsFolderWhenExportsCommandOptionSet()
        {
            var reader  = new Mock<IFileReader>();
            var context = new MockXrmTaskContext();

            reader.Setup(x => x.GetSolutionsInExportFolder()).Returns(new[] { "solution1.zip", "solution2.zip", "conn:connectionName" });
            reader.Setup(x => x.FileExists("solution1.zip")).Returns(true);
            reader.Setup(x => x.FileExists("solution2.zip")).Returns(true);
            var command = new ImportSolutionCommandLine(new[] { "import", "--exports", "conn:connectionName" }, reader.Object);
            var task = new ImportSolutionTask(command);

            task.Execute(context.Object);

            context.Service.Verify(x=>x.Execute(It.IsAny<ImportSolutionRequest>()),Times.Exactly(2));
            reader.Verify(x=>x.ReadAllBytes("solution1.zip"),Times.Once);
            reader.Verify(x=>x.ReadAllBytes("solution2.zip"),Times.Once);
        }

        [Fact]
        public void ExecutesImportRequestWhenCommandIsValid()
        {
            var reader = new Mock<IFileReader>();
            var context = new MockXrmTaskContext();

            const string filePath = @"c:\path\to\file.zip";

            var command = new ImportSolutionCommandLine(new[] { "import", filePath, "conn:connectionName" }, reader.Object);
            
            reader.Setup(x => x.ReadAllBytes(filePath)).Returns(new byte[] { });
            reader.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

            IXrmTask task = new ImportSolutionTask(command);
            task.Execute(context.Object);

            context.Service.Verify(x => x.Execute(It.IsAny<ImportSolutionRequest>()),Times.Once());
            Assert.Equal(filePath,command.GetSolutionFilePaths().Single());
        }

        [Fact]
        public void DoesNotAttemptToImportSolutionWhenHelpOptionSpecified()
        {
            var context = new MockXrmTaskContext();
            var commandLine = new ImportSolutionCommandLine(new[] { "import", "--help", "conn:connectionName" }, new Mock<IFileReader>().Object);
            var task        = new ImportSolutionTask(commandLine);
            
            task.Execute(context.Object);
            
            context.Service.Verify(x=>x.Execute(It.IsAny<OrganizationRequest>()),Times.Never);
        }

        [Fact]
        public void ImportsOnlyTheFileSpecifiedWhenReadingFromExportsFolder()
        {
            //TODO:  Noticed a bug whilst using tool that started reading all solutions rather than just the one specified
        }
    }
}
