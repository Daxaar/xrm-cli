using System.IO;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using System;
using System.Linq;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class ImportSolutionCommandLineTests
    {
        [TestMethod]
        public void CanReadPublishArgument()
        {
            var commandLine = new ImportSolutionCommandLine(new[] {"import"," filename ","nopublish"}, new Mock<IFileReader>().Object);

            Assert.IsTrue(commandLine.Publish);
        }

        [TestMethod]
        public void CanReadActivateProcessesArgument()
        {
            var commandLine = new ImportSolutionCommandLine(new[] {"import"," filename"," noactivate"}, new Mock<IFileReader>().Object);
            Assert.IsTrue(commandLine.ActivateProcesses);
        }

        [TestMethod]
        public void DefaultArgumentsAreTrueWhenNotSpecified()
        {
            var commandLine = new ImportSolutionCommandLine(new[] {"import","filename"}, new Mock<IFileReader>().Object);
            Assert.IsTrue(commandLine.ActivateProcesses);
            Assert.IsTrue(commandLine.Publish);
        }

        [TestMethod]
        public void CanReadFilenameArgument()
        {
            const string filename = @"c:\path\to\file.zip";
            var fileReader = new Mock<IFileReader>();
            fileReader.SetReturnsDefault(true);
            var commandLine = new ImportSolutionCommandLine(new[]{"import ", filename}, fileReader.Object);
            Assert.AreEqual(commandLine.GetSolutionFilePaths().Single(), filename);
        }

        [TestMethod]
        public void FindsFileInInSameDirectoryAsExecutableWhenOnlyFilenameProvided()
        {
            string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filename.zip");
            var fileReader  = new Mock<IFileReader>();
            var context     = new Mock<IXrmTaskContext>();
            context.Setup(x => x.Log).Returns(new Mock<ILog>().Object);
            context.Setup(x => x.Service).Returns(new Mock<IOrganizationService>().Object);

            fileReader.Setup(x => x.FileExists(defaultPath)).Returns(true);
            var commandline = new ImportSolutionCommandLine(new string[] { "import", "filename.zip" }, fileReader.Object);
            var task = new ImportSolutionTask(commandline);

            task.Execute(context.Object);
                
            fileReader.Verify(x => x.ReadAllBytes(It.IsAny<string>()));
            
        }
        [TestMethod]
        public void FindsFileInImportFolderWhenOnlyFilenameProvided()
        {
            string defaultPath  = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Import","filename.zip") ;
            var fileReader      = new Mock<IFileReader>();
            var context = new Mock<IXrmTaskContext>();
            context.Setup(x => x.Log).Returns(new Mock<ILog>().Object);
            context.Setup(x => x.Service).Returns(new Mock<IOrganizationService>().Object);

            fileReader.Setup(x => x.FileExists(defaultPath)).Returns(true);
            var commandline = new ImportSolutionCommandLine(new[] { "import", "filename.zip" }, fileReader.Object);
            var task = new ImportSolutionTask(commandline);

            task.Execute(context.Object);

            fileReader.Verify(x=>x.ReadAllBytes(defaultPath));
        }

        [TestMethod]
        public void AcceptsMultipleSolutionNamesOnCommandLine()
        {
            var reader = new Mock<IFileReader>();
            var context = new Mock<IXrmTaskContext>();
            context.Setup(x => x.Log).Returns(new Mock<ILog>().Object);
            var service = new Mock<IOrganizationService>();
            
            context.Setup(x => x.Service).Returns(service.Object);

            reader.SetReturnsDefault(true);
            var commandline = new ImportSolutionCommandLine(new[] {"import", "filename1,filename2"}, reader.Object);
            var task = new ImportSolutionTask(commandline);
            task.Execute(context.Object);

            service.Verify(x=>x.Execute(It.IsAny<ImportSolutionRequest>()),Times.Exactly(2));

        }
        [TestMethod]
        public void AddsZipExtensionWhenOnlySolutionNameProvided()
        {
            var reader = new Mock<IFileReader>();
            reader.SetReturnsDefault(true);
            var commandline = new ImportSolutionCommandLine(new[] { "import", "filename1" }, reader.Object);
            Assert.IsTrue(commandline.GetSolutionFilePaths().Single().EndsWith(".zip"));
        }

        [TestMethod]
        public void FindsFilesInExportFolderWhenExportsOptionSpecified()
        {
            var reader = new Mock<IFileReader>();
            var service = new Mock<IOrganizationService>();

            var command = new ImportSolutionCommandLine(new[] {"import", "--exports"},reader.Object);

            var paths = command.GetSolutionFilePaths().ToList();
            reader.Verify(x=>x.GetSolutionsInExportFolder(),Times.Once);

        }

        [TestMethod]
        public void UsesAllSolutionFilesInExportsFolderWhenExportsCommandOptionSet()
        {
            var reader = new Mock<IFileReader>();
            reader.Setup(x => x.GetSolutionsInExportFolder()).Returns(new[] { @"Exports\solution1.zip", @"Exports\solution2.zip" });
            reader.Setup(x => x.FileExists(@"Exports\solution1.zip")).Returns(true);
            reader.Setup(x => x.FileExists(@"Exports\solution2.zip")).Returns(true);
            var command = new ImportSolutionCommandLine(new[] { "import", "--exports" }, reader.Object);

            Assert.IsTrue(command.GetSolutionFilePaths().Contains(@"Exports\solution1.zip"));
            Assert.IsTrue(command.GetSolutionFilePaths().Contains(@"Exports\solution2.zip"));
        }

        [TestMethod]
        public void ShowHelpOptionIsTrueWhenSpecifiedAsArgument()
        {
            var command = new ImportSolutionCommandLine(new[] { "import", "--help" }, new Mock<IFileReader>().Object);
            Assert.IsTrue(command.ShowHelp);
        }

        [TestMethod]
        public void SetsOverwriteUnmanagedTrueWhenArgumentSpecified()
        {
            var command = new ImportSolutionCommandLine(new[] { "import", "--overwrite" }, new Mock<IFileReader>().Object);
            Assert.IsTrue(command.OverwriteUmanaged);
        }

    }
}
