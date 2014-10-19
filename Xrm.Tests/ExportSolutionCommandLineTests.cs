using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Octono.Xrm.Tasks;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class ExportSolutionCommandLineTests
    {
        [TestMethod]
        public void SetsIncrementVersionBeforeExportTrueWhenArgumentIncluded()
        {
            const string incrementArgument = "-i";
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1", incrementArgument });
            Assert.IsTrue(command.IncrementVersionBeforeExport);
        }

        [TestMethod]
        public void SetsManagedPropertyTrueWhenArgumentIncluded()
        {
            const string managedArg = "-m";
            var command = new ExportSolutionCommandLine(new[]{"export","solution1",managedArg});
            Assert.IsTrue(command.Managed);
        }

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
            const string path = @"c:\export\specificfilename.zip";
            var command = new ExportSolutionCommandLine(new[] { "export", "sol1",path });

            Assert.AreEqual(command.BuildExportPath("sol1"),path);
        }

        [TestMethod]
        public void CreatesSolutionFilenameWithVersionNumber()
        {
            var command = new ExportSolutionCommandLine(new[]{"export","solution"});
            string path = command.BuildExportPath("solution", "0.0.0.1");
            Assert.AreEqual("Export\\solution-0-0-0-1.zip",path);
        }

        [TestMethod]
        public void AddsManagedSuffixWhenExportedSolutionIsManaged()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution","-m" });
            string path = command.BuildExportPath("solution", "0.0.0.1");
            Assert.AreEqual("Export\\solution-0-0-0-1_managed.zip", path);
        }        
        [TestMethod]
        public void DoesNotAddManagedSuffixWhenExportedSolutionIsUnmanaged()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution" });
            string path = command.BuildExportPath("solution", "0.0.0.1");
            Assert.AreEqual("Export\\solution-0-0-0-1.zip", path);
        }
    }
}
