using System;
using Moq;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;
using Xunit;


namespace Octono.Xrm.Tests
{    
    public class XrmTaskFactoryTests
    {
        [Fact]
        public void WhenCommandIsDeployReturnsDeployWebResourceTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);
            IXrmTask task = factory.CreateTask(new[] { "deploy", "filename.js","conn:connectionName" });
            Assert.IsType<DeployWebResourceTask>(task);            
        }

        [Fact]
        public void WhenCommandIsImport_ReturnsImportSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);
            IXrmTask task = factory.CreateTask(new[] { "import", "filename", "conn:connectionName" });
            Assert.IsType<ImportSolutionTask>(task);
        }


        [Fact]
        public void WhenCommandIsExport_ReturnsExportSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);
            IXrmTask task = factory.CreateTask(new[] { "export", "filename", "conn:connectionName" });
            Assert.IsType<ExportSolutionTask>(task);            
        }

        [Fact]
        public void WhenValidCommandIsNotSpecifiedThrowsException()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);
            Assert.Throws<InvalidOperationException>(() => factory.CreateTask(new[] { "invalidcommand" }));
        }   
        [Fact]
        public void AcceptsOrganizationServiceInConstructor()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);

            Assert.NotNull(factory);
        }

        [Fact]
        public void WhenCommandIsPublish_ReturnsPublishSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);
            IXrmTask task = factory.CreateTask(new[] { "publish", "conn:connectionName" });
            Assert.IsType<PublishSolutionTask>(task); 
        }

        [Theory]
        [InlineData("publish", typeof(PublishSolutionTask))]
        [InlineData("addconnection", typeof(AddConnectionTask))]
        [InlineData("connectiontest", typeof(ConnectionTestTask))]
        [InlineData("copy", typeof(CopyRecordsTask))]
        [InlineData("deletesolution", typeof(DeleteSolutionTask))]
        public void ReturnsCorrectTaskForSpecifiedCommand(string command, Type taskType)
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);
            IXrmTask task = factory.CreateTask(new[] { command, "conn:connectionName" });
            Assert.IsType(taskType, task);
        }

    }
}
