using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Octono.Xrm.ConsoleTaskRunner;
using Octono.Xrm.Tasks;
using xrm;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class ServerConnectCommandLineTests
    {
        [TestMethod]
        public void CanReadServerNameArgument()
        {
            var connection = new ServerConnection(new List<string>(){"import"," server:servername","org:orgname","port:5555"},new ConsoleLogger());
            Assert.AreEqual("servername", connection.ServerName);
        }

        [TestMethod]
        public void CanReadOrgNameArgument()
        {
            var connection = new ServerConnection(new List<string>() { "import", "server:servername", "org:orgname", "port:5555" },new ConsoleLogger() );
            Assert.AreEqual("orgname", connection.OrganizationName);
        }

        [TestMethod]
        public void CanReadPortNumberArgument()
        {
            var connection = new ServerConnection(new List<string>() { "import", "server:servername", "org:orgname", "port:5555" },new ConsoleLogger());
            Assert.AreEqual("5555", connection.Port);
        }

        [TestMethod]
        public void UsesLocalhostWhenServerNameIsNotSpecified()
        {
            var validCommandArgsWithoutServerName = new List<string>() {"taskname", "org:orgname"};

            var connection = new ServerConnection(validCommandArgsWithoutServerName,new ConsoleLogger());

            Assert.AreEqual("localhost",connection.ServerName);
        }
    }
}