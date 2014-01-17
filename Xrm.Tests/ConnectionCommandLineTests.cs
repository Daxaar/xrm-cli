using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
}