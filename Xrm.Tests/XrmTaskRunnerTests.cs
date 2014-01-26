using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Octono.Xrm.ConsoleTaskRunner;
using Octono.Xrm.Tasks;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class XrmTaskRunnerTests
    {
        [TestMethod]
        public void RunnerDisplayErrorMessageToConsoleWhenTaskFails()
        {
            var fileReader = new Mock<IFileReader>();
            fileReader.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            var args = new[] { "import", "filename" };
            var runner = new XrmTaskRunner(new Mock<ILog>().Object);
            runner.Run(args);
        }

        [TestMethod]
        public void RunnerPassesCommandLineArgumentsToXrmFactory()
        {
            var fileReader = new Mock<IFileReader>();
            fileReader.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            var args = new[] {"import", "filename"};
            var runner = new XrmTaskRunner(new ConsoleLogger());
            runner.Run(args);
        }
            
        [TestMethod]
        public void RunnerCanAcceptFurtherCommandsUponCompletionOfPrevious()
        {
            var fileReader = new Mock<IFileReader>();
            fileReader.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            var runner = new XrmTaskRunner(new ConsoleLogger());
            runner.Run(new[] { "import", "filename" });
        }

        [TestMethod]
        public void ThrowsExceptionIfFirstCommandDoesNotContainConnectionInfo()
        {
        }
    }
}