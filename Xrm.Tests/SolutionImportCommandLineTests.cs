using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xrm.Tests
{
    [TestClass]
    public class SolutionImportCommandLineTests
    {
        [TestMethod]
        public void CanReadPublishArgument()
        {
            var parser = new SolutionImportCommandLine(new Mock<IFileReader>().Object);

            parser.Parse("import filename nopublish");

            Assert.IsTrue(parser.Publish);
        }

        [TestMethod]
        public void CanReadActivateProcessesArgument()
        {
            var parser = new SolutionImportCommandLine(new Mock<IFileReader>().Object);
            parser.Parse("import filename noactivate");
            Assert.IsTrue(parser.ActivateProcesses);
        }

        [TestMethod]
        public void DefaultArgumentsAreTrueWhenNotSpecified()
        {
            var parser = new SolutionImportCommandLine(new Mock<IFileReader>().Object);
            parser.Parse("import filename");
            Assert.IsTrue(parser.ActivateProcesses);
            Assert.IsTrue(parser.Publish);
        }

        [TestMethod]
        public void CanReadFilenameArgument()
        {
            var parser = new SolutionImportCommandLine(new Mock<IFileReader>().Object);

            string filename = @"c:\path\to\file.zip";

            parser.Parse("import " + filename);

            Assert.AreEqual(parser.SolutionFilePath, filename);
        }

    }
}
