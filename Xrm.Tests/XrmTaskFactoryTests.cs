using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Xrm.Sdk;
using Moq;
using Octono.Xrm.ConsoleTaskRunner;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class XrmTaskFactoryTests
    {
        [TestMethod]
        public void WhenCommandIsDeployReturnsDeployWebResourceTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);
            IXrmTask task = factory.CreateTask(new[] { "deploy", "filename.js" });
            Assert.IsInstanceOfType(task, typeof(DeployWebResourceTask));            
        }

        [TestMethod]
        public void WhenCommandIsImport_ReturnsImportSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);
            IXrmTask task = factory.CreateTask(new[] { "import", "filename" });
            Assert.IsInstanceOfType(task, typeof(ImportSolutionTask));
        }


        [TestMethod]
        public void WhenCommandIsExport_ReturnsExportSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);
            IXrmTask task = factory.CreateTask(new[] { "export", "filename" });
            Assert.IsInstanceOfType(task, typeof(ExportSolutionTask));            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenValidCommandIsNotSpecifiedThrowsException()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);
            factory.CreateTask(new[] { "invalidcommand" });
        }   
        [TestMethod]
        public void AcceptsOrganizationServiceInConstructor()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);

            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void WhenCommandIsPublish_ReturnsPublishSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object);
            IXrmTask task = factory.CreateTask(new[] { "publish" });
            Assert.IsInstanceOfType(task, typeof(PublishSolutionTask)); 
        }

    }
}
