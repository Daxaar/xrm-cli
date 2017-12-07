using System.Linq;
using Octono.Xrm.Tasks;
using Xunit;


namespace Octono.Xrm.Tests
{
    
    public class ExportSolutionCommandLineTests
    {
        [Fact]
        public void SetsIncrementVersionBeforeExportTrueWhenArgumentIncluded()
        {
            const string incrementArgument = "-i";
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1", incrementArgument, "conn:connectionName" });
            Assert.True(command.IncrementVersionBeforeExport);
        }

        [Fact]
        public void SetsManagedPropertyTrueWhenArgumentIncluded()
        {
            const string managedArg = "-m";
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1", managedArg, "conn:connectionName" });
            Assert.True(command.Managed);
        }

        [Fact]
        public void CanReadCommaSeparatedSolutionFilesArgument()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1,solution2", "conn:connectionName" });
            Assert.Contains("solution1", command.SolutionNames);
            Assert.Contains("solution2", command.SolutionNames);
        }

        [Fact]
        public void ExportsToExportFolderWhenPathNotSpecified()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1", "conn:connectionName" });
            Assert.Equal( @"Export\solution1.zip",command.BuildExportPath("solution1"));
        }

        [Fact]
        public void AddsZipExtensionToExportFilePath()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1", "conn:connectionName" });
            Assert.Equal(@"Export\solution1.zip", command.BuildExportPath("solution1"));
        }

        [Fact]
        public void UsesExportPathWhenSpecified()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1", @"c:\path\to\file", "conn:connectionName" });
            Assert.Equal(@"c:\path\to\file\solution1.zip",command.BuildExportPath("solution1"));
        }

        [Fact]
        public void SplitsCommaSeparatedListOfSolutions()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1,solution2", @"out:c:\path\to\file", "conn:connectionName" });
            Assert.Equal(2, command.SolutionNames.Count());
            Assert.Equal("solution1",command.SolutionNames.ToList()[0]);
            Assert.Equal("solution2", command.SolutionNames.ToList()[1]);
        }

        [Fact]
        public void UsesExportFolderWhenThirdParameterIsNotExportPath()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution1", "connectionName" });
            Assert.Equal(@"Export\solution1.zip", command.BuildExportPath("solution1"));
            
        }

        [Fact]
        public void UsesFilenameForSolutionWhenWhenExportPathSpecifiesFilename()
        {
            const string path = @"c:\export\specificfilename.zip";
            var command = new ExportSolutionCommandLine(new[] { "export", "sol1", path, "conn:connectionName" });

            Assert.Equal(command.BuildExportPath("sol1"),path);
        }

        [Fact]
        public void CreatesSolutionFilenameWithVersionNumber()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution", "conn:connectionName" });
            string path = command.BuildExportPath("solution", "0.0.0.1");
            Assert.Equal("Export\\solution-0-0-0-1.zip",path);
        }

        [Fact]
        public void AddsManagedSuffixWhenExportedSolutionIsManaged()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution", "-m", "conn:connectionName" });
            string path = command.BuildExportPath("solution", "0.0.0.1");
            Assert.Equal("Export\\solution-0-0-0-1_managed.zip", path);
        }        
        [Fact]
        public void DoesNotAddManagedSuffixWhenExportedSolutionIsUnmanaged()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "solution", "conn:connectionName" });
            string path = command.BuildExportPath("solution", "0.0.0.1");
            Assert.Equal("Export\\solution-0-0-0-1.zip", path);
        }
    }
}
