using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var commandLine = new ImportSolutionCommandLine(new[]{"import ", filename}, new Mock<IFileReader>().Object);
            Assert.AreEqual(commandLine.SolutionFilePath, filename);
        }

    }
}
