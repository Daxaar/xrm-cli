using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;
using Octono.Xrm.Tests.Builders;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class XrmTaskFactoryTests
    {
        [TestMethod]
        public void WhenCommandIsDeployReturnsDeployWebResourceTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object,MockConfigurationManagerBuilder.Build());
            IXrmTask task = factory.CreateTask(new[] { "deploy", "filename.js" });
            Assert.IsInstanceOfType(task, typeof(DeployWebResourceTask));            
        }

        [TestMethod]
        public void WhenCommandIsImport_ReturnsImportSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, MockConfigurationManagerBuilder.Build());
            IXrmTask task = factory.CreateTask(new[] { "import", "filename" });
            Assert.IsInstanceOfType(task, typeof(ImportSolutionTask));
        }


        [TestMethod]
        public void WhenCommandIsExport_ReturnsExportSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, MockConfigurationManagerBuilder.Build());
            IXrmTask task = factory.CreateTask(new[] { "export", "filename" });
            Assert.IsInstanceOfType(task, typeof(ExportSolutionTask));            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenValidCommandIsNotSpecifiedThrowsException()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, MockConfigurationManagerBuilder.Build());
            factory.CreateTask(new[] { "invalidcommand" });
        }   
        [TestMethod]
        public void AcceptsOrganizationServiceInConstructor()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, MockConfigurationManagerBuilder.Build());

            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void WhenCommandIsPublish_ReturnsPublishSolutionTask()
        {
            var factory = new XrmTaskFactory(new Mock<IFileReader>().Object, new Mock<IFileWriter>().Object, MockConfigurationManagerBuilder.Build());
            IXrmTask task = factory.CreateTask(new[] { "publish" });
            Assert.IsInstanceOfType(task, typeof(PublishSolutionTask)); 
        }

    }
}
