using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Moq;

namespace Xrm.Tests
{
    [TestClass]
    public class ConnectionCommandLineTests
    {
        [TestMethod]
        public void CanReadServerNameArgument()
        {
            var connection = new ServerConnection(new List<string>(){"import"," server:servername","org:orgname","port:5555"});
            Assert.AreEqual("servername", connection.ServerName);
        }

        [TestMethod]
        public void CanReadOrgNameArgument()
        {
            var connection = new ServerConnection(new List<string>() { "import", "server:servername", "org:orgname", "port:5555" });
            Assert.AreEqual("orgname", connection.OrganizationName);
        }

        [TestMethod]
        public void CanReadPortNumberArgument()
        {
            var connection = new ServerConnection(new List<string>() { "import", "server:servername", "org:orgname", "port:5555" });
            Assert.AreEqual("5555", connection.Port);
        }

        [TestMethod]
        public void UsesLocalhostWhenServerNameIsNotSpecified()
        {
            var validCommandArgsWithoutServerName = new List<string>() {"taskname", "org:orgname"};

            var connection = new ServerConnection(validCommandArgsWithoutServerName);

            Assert.AreEqual("localhost",connection.ServerName);
        }
    }

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
            var args = new[] {"import", "filename"};
            var runner = new XrmTaskRunner(new XrmTaskFactory(args, new Mock<IFileReader>().Object, new Mock<IOrganizationService>().Object),
                                            new Mock<ILog>().Object);
            runner.Run();

        }

        [TestMethod]
        public void RunnerCanAcceptFurtherCommandsUponCompletionOfPrevious()
        {
            var runner = new XrmTaskRunner( new XrmTaskFactory(new[] {"import", "filename"},
                                            new Mock<IFileReader>().Object, new Mock<IOrganizationService>().Object),
                                            new Mock<ILog>().Object);
            runner.Run();
        }

        [TestMethod]
        public void ThrowsExceptionIfFirstCommandDoesNotContainConnectionInfo()
        {
        }
    }

    class XrmTaskRunnerTestsImpl : XrmTaskRunnerTests
    {
    }
}