using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Moq;

namespace Xrm.Tests
{
    [TestClass]
    public class XrmTaskRunnerTests
    {
        //[TestMethod]
        //public void RunnerCanAcceptCommandLineArguments()
        //{

        //    var runner  = new XrmTaskRunner(new Mock<IXrmTaskFactory>().Object,new Mock<ILog>().Object);
        //    runner.Run();
        //}

        [TestMethod]
        public void RunnerPassesCommandLineArgumentsToXrmFactory()
        {
            var fileReader = new Mock<IFileReader>();
            fileReader.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            var args = new[] {"import", "filename"};
            var runner = new XrmTaskRunner(new XrmTaskFactory(fileReader.Object, new Mock<IOrganizationService>().Object),
                                            new Mock<ILog>().Object);
            runner.Run(args);

        }

        [TestMethod]
        public void RunnerCanAcceptFurtherCommandsUponCompletionOfPrevious()
        {
            var fileReader = new Mock<IFileReader>();
            fileReader.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            var runner = new XrmTaskRunner( new XrmTaskFactory(
                                            fileReader.Object, new Mock<IOrganizationService>().Object),
                                            new Mock<ILog>().Object);
            runner.Run(new[] { "import", "filename" });
        }

        [TestMethod]
        public void ThrowsExceptionIfFirstCommandDoesNotContainConnectionInfo()
        {
        }
    }
}