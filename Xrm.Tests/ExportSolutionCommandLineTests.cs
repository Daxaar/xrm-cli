using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using xrm;

namespace Xrm.Tests
{
    [TestClass]
    public class ExportSolutionCommandLineTests
    {
        [TestMethod]
        public void CanReadCommaSeparatedSolutionFilesArgument()
        {
            var command = new ExportSolutionCommandLine(new[] {"export","solution1,solution2"});
            Assert.IsTrue(command.SolutionNames.Contains("solution1"));
            Assert.IsTrue(command.SolutionNames.Contains("solution2"));
        }

        [TestMethod]
        public void ExportsToExportFolderWhenPathNotSpecified()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1" });
            Assert.AreEqual( @"Export\solution1.zip",command.BuildExportPath("solution1"));
        }

        [TestMethod]
        public void AddsZipExtensionToExportFilePath()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1" });
            Assert.AreEqual(@"Export\solution1.zip", command.BuildExportPath("solution1"));
        }

        [TestMethod]
        public void UsesExportPathWhenSpecified()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1",@"out:c:\path\to\file" });
            Assert.AreEqual(@"c:\path\to\file\solution1.zip",command.BuildExportPath("solution1"));
        }

        [TestMethod]
        public void UsesExportFolderWhenThirdParameterIsNotExportPath()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1", "org:orgname" });
            Assert.AreEqual(@"Export\solution1.zip", command.BuildExportPath("solution1"));
            
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

    }
}
