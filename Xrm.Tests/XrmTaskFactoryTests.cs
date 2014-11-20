using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void CanDeserializeJsonConfiguration()
        {
            
        }
    }
    [TestClass]
    public class XrmTaskFactoryTests
    {
        [TestMethod]
        public void WhenCommandIsDeployReturnsDeployWebResourceTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object,new Mock<IXrmConfiguration>().Object);
            IXrmTask task = factory.CreateTask(new[] { "deploy", "filename.js","conn:connectionName" });
            Assert.IsInstanceOfType(task, typeof(DeployWebResourceTask));            
        }

        [TestMethod]
        public void WhenCommandIsImport_ReturnsImportSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, new Mock<IXrmConfiguration>().Object);
            IXrmTask task = factory.CreateTask(new[] { "import", "filename", "conn:connectionName" });
            Assert.IsInstanceOfType(task, typeof(ImportSolutionTask));
        }


        [TestMethod]
        public void WhenCommandIsExport_ReturnsExportSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, new Mock<IXrmConfiguration>().Object);
            IXrmTask task = factory.CreateTask(new[] { "export", "filename", "conn:connectionName" });
            Assert.IsInstanceOfType(task, typeof(ExportSolutionTask));            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenValidCommandIsNotSpecifiedThrowsException()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, new Mock<IXrmConfiguration>().Object);
            factory.CreateTask(new[] { "invalidcommand" });
        }   
        [TestMethod]
        public void AcceptsOrganizationServiceInConstructor()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, new Mock<IXrmConfiguration>().Object);

            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void WhenCommandIsPublish_ReturnsPublishSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, new Mock<IXrmConfiguration>().Object);
            IXrmTask task = factory.CreateTask(new[] { "publish", "conn:connectionName" });
            Assert.IsInstanceOfType(task, typeof(PublishSolutionTask)); 
        }

    }
}
