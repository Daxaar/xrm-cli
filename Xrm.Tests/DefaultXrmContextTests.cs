//using System;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.Xrm.Sdk;
//using Moq;
//using Octono.Xrm.Tasks;
//using Octono.Xrm.Tasks.IO;

//namespace Octono.Xrm.Tests
//{
//    [TestClass]
//    public class DefaultXrmContextTests
//    {
//        [TestMethod]
//        [ExpectedException(typeof(InvalidOperationException))]
//        public void ThrowsExceptionWhenRequestedConnectionIsNotInConfigFile()
//        {
//            var config  = new Mock<IConfigurationManager>();
//            var factory = new Mock<IXrmServiceFactory>();
//            config.Setup(x => x.ConnectionStrings).Returns(new Dictionary<string,ConnectionInfo>());
//            var context = new XrmTaskContext();

//            context.CreateService("DoesNotExistInConfig");
//        }

//        [TestMethod]
//        public void ReturnsAnOrganisationServiceWhenConfigExists()
//        {
//            var log     = new Mock<ILog>();
//            var service = new Mock<IOrganizationService>();
//            var config  = new Mock<IConfigurationManager>();
//            var context = new XrmTaskContext(service.Object, log.Object, config.Object);
//            var connection = new ConnectionInfo
//                {
//                    Name = "ValidConnection",
//                    Port = 80,
//                    Protocol = "http",
//                    ServerName = "server",
//                    Organisation = "organisation"
//                };

//            var connections = new Dictionary<string, ConnectionInfo>()
//                {
//                    {connection.Name,connection }
//                };

//            config.Setup(x => x.ConnectionStrings).Returns(connections);

//            context.CreateService(connection.Name);
//        }
//    }
//}