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
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object,new Mock<IXrmConfiguration>().Object);
            IXrmTask task = factory.CreateTask(new[] { "deploy", "filename.js","conn:connectionName" });
            Assert.IsType(typeof(DeployWebResourceTask), task);            
        }

        [Fact]
        public void WhenCommandIsImport_ReturnsImportSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, new Mock<IXrmConfiguration>().Object);
            IXrmTask task = factory.CreateTask(new[] { "import", "filename", "conn:connectionName" });
            Assert.IsType(typeof(ImportSolutionTask), task);
        }


        [Fact]
        public void WhenCommandIsExport_ReturnsExportSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, new Mock<IXrmConfiguration>().Object);
            IXrmTask task = factory.CreateTask(new[] { "export", "filename", "conn:connectionName" });
            Assert.IsType(typeof(ExportSolutionTask), task);            
        }

        [Fact]
        public void WhenValidCommandIsNotSpecifiedThrowsException()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, new Mock<IXrmConfiguration>().Object);
            Assert.Throws<InvalidOperationException>(() => factory.CreateTask(new[] { "invalidcommand" }));
        }   
        [Fact]
        public void AcceptsOrganizationServiceInConstructor()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, new Mock<IXrmConfiguration>().Object);

            Assert.NotNull(factory);
        }

        [Fact]
        public void WhenCommandIsPublish_ReturnsPublishSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, new Mock<IXrmConfiguration>().Object);
            IXrmTask task = factory.CreateTask(new[] { "publish", "conn:connectionName" });
            Assert.IsType(typeof(PublishSolutionTask), task); 
        }

    }
}
