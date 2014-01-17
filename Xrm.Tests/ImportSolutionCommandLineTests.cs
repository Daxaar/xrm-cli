using System.IO;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xrm.Tests
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
            Assert.AreEqual(commandLine.SolutionFilePath, filename);
        }

        [TestMethod]
        public void FindsFileInInSameDirectoryAsExecutableWhenOnlyFilenameProvided()
        {
            string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filename.zip");
            var fileReader = new Mock<IFileReader>();
            fileReader.Setup(x => x.FileExists(defaultPath)).Returns(true);
            var commandline = new ImportSolutionCommandLine(new string[] { "import", "filename.zip" }, fileReader.Object);
            var task = new ImportSolutionTask(commandline, new Mock<IOrganizationService>().Object, new Mock<ILog>().Object);

            task.Execute();
                
            fileReader.Verify(x => x.ReadAllBytes(It.IsAny<string>()));
            
        }
        [TestMethod]
        public void FindsFileInImportFolderWhenOnlyFilenameProvided()
        {
            string defaultPath  = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Import","filename.zip") ;
            var fileReader      = new Mock<IFileReader>();
            fileReader.Setup(x => x.FileExists(defaultPath)).Returns(true);
            var commandline = new ImportSolutionCommandLine(new[] { "import", "filename.zip" }, fileReader.Object);
            var task = new ImportSolutionTask(commandline, new Mock<IOrganizationService>().Object,new Mock<ILog>().Object);

            task.Execute();

            fileReader.Verify(x=>x.ReadAllBytes(defaultPath));
        }

        [TestMethod]
        public void AcceptsMultipleSolutionNamesOnCommandLine()
        {
            var reader = new Mock<IFileReader>();
            var commandline = new ImportSolutionCommandLine(new[] {"import", "filename1,filename2"}, reader.Object);
            var service = new Mock<IOrganizationService>();
            var task = new ImportSolutionTask(commandline, service.Object, new Mock<ILog>().Object);
            task.Execute();

            service.Verify(x=>x.Execute(It.IsAny<ImportSolutionRequest>()),Times.Exactly(2));

        }

    }
}
