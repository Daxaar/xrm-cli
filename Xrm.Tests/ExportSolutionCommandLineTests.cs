using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Octono.Xrm.Tasks;

namespace Octono.Xrm.Tests
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
        public void SplitsCommaSeparatedListOfSolutions()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1,solution2", @"out:c:\path\to\file" });
            Assert.AreEqual(2, command.SolutionNames.Count());
            Assert.AreEqual("solution1",command.SolutionNames.ToList()[0]);
            Assert.AreEqual("solution2", command.SolutionNames.ToList()[1]);
        }

        [TestMethod]
        public void UsesExportFolderWhenThirdParameterIsNotExportPath()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1", "org:orgname" });
            Assert.AreEqual(@"Export\solution1.zip", command.BuildExportPath("solution1"));
            
        }

        [TestMethod]
        public void UsesFilenameForSolutionWhenWhenExportPathSpecifiesFilename()
        {
            var reader = new Mock<IFileReader>();
            const string path = @"c:\export\specificfilename.zip";
            var command = new ExportSolutionCommandLine(new[] { "export", "sol1",path });

            Assert.AreEqual(command.BuildExportPath("sol1"),path);
        }

    }
}
