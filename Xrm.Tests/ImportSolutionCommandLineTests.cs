using System.IO;
using Microsoft.Crm.Sdk.Messages;
using Moq;
using System;
using System.Linq;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;
using Octono.Xrm.Tests.Builders;
using Xunit;

namespace Octono.Xrm.Tests
{
    public class ImportSolutionCommandLineTests
    {
        [Fact]
        public void CanReadPublishArgument()
        {
            var commandLine = new ImportSolutionCommandLine(new[] { "import", " filename ", "nopublish", "conn:connectionName" }, new Mock<IFileReader>().Object);

            Assert.True(commandLine.Publish);
        }

        [Fact]
        public void CanReadActivateProcessesArgument()
        {
            var commandLine = new ImportSolutionCommandLine(new[] { "import", " filename", " noactivate", "conn:connectionName" }, new Mock<IFileReader>().Object);
            Assert.True(commandLine.ActivateProcesses);
        }

        [Fact]
        public void DefaultArgumentsAreTrueWhenNotSpecified()
        {
            var commandLine = new ImportSolutionCommandLine(new[] { "import", "filename", "conn:connectionName" }, new Mock<IFileReader>().Object);
            Assert.True(commandLine.ActivateProcesses);
            Assert.True(commandLine.Publish);
        }

        [Fact]
        public void CanReadFilenameArgument()
        {
            const string filename = @"c:\path\to\file.zip";
            var fileReader = new Mock<IFileReader>();
            fileReader.SetReturnsDefault(true);
            var commandLine = new ImportSolutionCommandLine(new[] { "import ", filename, "conn:connectionName" }, fileReader.Object);
            Assert.Equal(commandLine.GetSolutionFilePaths().Single(), filename);
        }

        [Fact]
        public void FindsFileInInSameDirectoryAsExecutableWhenOnlyFilenameProvided()
        {
            string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filename.zip");
            var fileReader  = new Mock<IFileReader>();
            var context = new MockXrmTaskContext();

            fileReader.Setup(x => x.FileExists(defaultPath)).Returns(true);
            var commandline = new ImportSolutionCommandLine(new[] { "import", "filename.zip", "conn:connectionName" }, fileReader.Object);
            var task = new ImportSolutionTask(commandline);

            task.Execute(context.Object);
                
            fileReader.Verify(x => x.ReadAllBytes(It.IsAny<string>()));
            
        }
        [Fact]
        public void FindsFileInImportFolderWhenOnlyFilenameProvided()
        {
            string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Import", "filename.zip");
            var fileReader      = new Mock<IFileReader>();
            var context         = new MockXrmTaskContext();

            fileReader.Setup(x => x.FileExists(defaultPath)).Returns(true);
            var commandline = new ImportSolutionCommandLine(new[] { "import", "filename.zip", "conn:connectionName" }, fileReader.Object);
            var task = new ImportSolutionTask(commandline);

            task.Execute(context.Object);

            fileReader.Verify(x=>x.ReadAllBytes(defaultPath));
        }

        [Fact]
        public void AcceptsMultipleSolutionNamesOnCommandLine()
        {
            var reader = new Mock<IFileReader>();
            var context = new MockXrmTaskContext();

            reader.SetReturnsDefault(true);
            var commandline = new ImportSolutionCommandLine(new[] { "import", "filename1,filename2", "conn:connectionName" }, reader.Object);
            var task = new ImportSolutionTask(commandline);
            task.Execute(context.Object);

            context.Service.Verify(x=>x.Execute(It.IsAny<ImportSolutionRequest>()),Times.Exactly(2));

        }
        [Fact]
        public void AddsZipExtensionWhenOnlySolutionNameProvided()
        {
            var reader = new Mock<IFileReader>();
            reader.SetReturnsDefault(true);
            var commandline = new ImportSolutionCommandLine(new[] { "import", "filename1", "conn:connectionName" }, reader.Object);
            Assert.True(commandline.GetSolutionFilePaths().Single().EndsWith(".zip"));
        }

        [Fact]
        public void FindsFilesInExportFolderWhenExportsOptionSpecified()
        {
            var reader  = new Mock<IFileReader>();
            var command = new ImportSolutionCommandLine(new[] { "import", "--exports", "conn:connectionName" }, reader.Object);

            command.GetSolutionFilePaths().ToList();
            reader.Verify(x=>x.GetSolutionsInExportFolder(),Times.Once);

        }

        [Fact]
        public void UsesAllSolutionFilesInExportsFolderWhenExportsCommandOptionSet()
        {
            var reader = new Mock<IFileReader>();
            reader.Setup(x => x.GetSolutionsInExportFolder()).Returns(new[] { @"Exports\solution1.zip", @"Exports\solution2.zip", "conn:connectionName" });
            reader.Setup(x => x.FileExists(@"Exports\solution1.zip")).Returns(true);
            reader.Setup(x => x.FileExists(@"Exports\solution2.zip")).Returns(true);
            var command = new ImportSolutionCommandLine(new[] { "import", "--exports", "conn:connectionName" }, reader.Object);

            Assert.True(command.GetSolutionFilePaths().Contains(@"Exports\solution1.zip"));
            Assert.True(command.GetSolutionFilePaths().Contains(@"Exports\solution2.zip"));
        }

        [Fact]
        public void ShowHelpOptionIsTrueWhenSpecifiedAsArgument()
        {
            var command = new ImportSolutionCommandLine(new[] { "import", "--help", "conn:connectionName" }, new Mock<IFileReader>().Object);
            Assert.True(command.ShowHelp);
        }

        [Fact]
        public void SetsOverwriteUnmanagedTrueWhenArgumentSpecified()
        {
            var command = new ImportSolutionCommandLine(new[] { "import", "--overwrite", "conn:connectionName" }, new Mock<IFileReader>().Object);
            Assert.True(command.OverwriteUmanaged);
        }

        [Fact]
        public void AcceptsConnectionNameAsLastArgument()
        {
            var command = new ImportSolutionCommandLine(new[] {"import", "solutionpath", "connectionname"},new Mock<IFileReader>().Object);
            Assert.Equal("connectionname",command.ConnectionName);
        }


    }
}
