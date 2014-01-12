using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Moq;

namespace Xrm.Tests
{
    [TestClass]
    public class XrmTaskFactoryTests
    {
        [TestMethod]
        public void WhenCommandIsImport_XrmTaskFactoryReturnsImportSolutionTask()
        {
            var factory = new XrmTaskFactory(new[] { "import" }, new Mock<IFileReader>().Object, new Mock<IOrganizationService>().Object);
            IXrmTask task = factory.CreateTask();
            Assert.IsInstanceOfType(task, typeof(SolutionImportTask));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenValidCommandIsNotSpecifiedThrowsException()
        {
            const string command = "invalidcommand randomarg";
            var factory = new XrmTaskFactory(new[] { command }, new Mock<IFileReader>().Object, new Mock<IOrganizationService>().Object);
            factory.CreateTask();
        }
        [TestMethod]
        public void AcceptsOrganizationServiceInConstructor()
        {
            var factory = new XrmTaskFactory(new[] {"taskname", "arg", "arg"}, new Mock<IFileReader>().Object,
                                             new Mock<IOrganizationService>().Object);

            Assert.IsNotNull(factory);
        }

    }
}
