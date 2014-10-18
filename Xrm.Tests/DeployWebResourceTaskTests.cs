using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Octono.Xrm.ConsoleTaskRunner;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class DeployWebResourceTaskTests
    {
        private const string Path = @"c:\test\ntt_contribution.js";
        private static readonly string[] Args = new[] {"deploy", Path };

        [TestMethod]
        public void DoesNotUpdateWebResourceWhenLocalFileIsEmptyAndForceFlagNotSpecified()
        {
            var reader  = new Mock<IFileReader>();
            var task    = new DeployWebResourceTask(new DeployWebResourceCommandLine(Args), reader.Object);
            var context = new Mock<IXrmTaskContext>();
            var service = new Mock<IOrganizationService>();
            
            context.Setup(x => x.Service).Returns(service.Object);
            context.Setup(x => x.Log).Returns(new Mock<ILog>().Object);
            
            task.Execute(context.Object);
            
            service.Verify(x=>x.Update(It.IsAny<Entity>()),Times.Never);
        }

        [TestMethod]
        public void ReadsWebResourceFileFromDisk()
        {

            var reader  = new Mock<IFileReader>();
            var task    = new DeployWebResourceTask(new DeployWebResourceCommandLine(Args),reader.Object);
            var context = new Mock<IXrmTaskContext>();
            var service = new Mock<IOrganizationService>();
            var collectionWithOneRecord = new EntityCollection(new[] { new Entity("webresource") });

            service.Setup(x => x.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(collectionWithOneRecord);
            context.Setup(x => x.Service).Returns(service.Object);
            
            task.Execute(context.Object);
            
            reader.Verify(x=>x.ReadAllBytes(Path),Times.Once);
        }

        [TestMethod]
        public void LoadsWebResourceFromServer()
        {
            var context         = new Mock<IXrmTaskContext>();
            var service         = new Mock<IOrganizationService>();
            var collectionWithOneRecord    = new EntityCollection(new[] { new Entity("webresource") });
            var reader          = new Mock<IFileReader>();
            var task            = new DeployWebResourceTask(new DeployWebResourceCommandLine(Args), reader.Object);

            service.Setup(x => x.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(collectionWithOneRecord);
            context.Setup(x => x.Service).Returns(service.Object);

            task.Execute(context.Object);
            
            service.Verify(x=>x.RetrieveMultiple(It.IsAny<QueryBase>()),Times.Once);
        }

        [TestMethod]
        public void UpdatesWebResourceContentAsBase64String()
        {
            const string webresourcecontent = "random javascript content";
            var context = new Mock<IXrmTaskContext>();
            var service = new Mock<IOrganizationService>();
            var reader  = new Mock<IFileReader>();
            reader.Setup(x => x.ReadAllBytes(Path)).Returns(Encoding.UTF8.GetBytes(webresourcecontent));
            var webresource = new Entity("webresource") { Attributes = new AttributeCollection() { { "name", "ntt_contribution" } } };

            var task = new DeployWebResourceTask(new DeployWebResourceCommandLine(Args), reader.Object);

            service.Setup(x => x.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(new EntityCollection(new[] { webresource } ));
            context.Setup(x => x.Service).Returns(service.Object);

            task.Execute(context.Object);

            service.Verify(x => x.RetrieveMultiple(It.IsAny<QueryBase>()), Times.Once);
            service.Verify(x => x.Update(webresource),Times.Once);
            Assert.AreEqual(webresourcecontent.ToBase64String(),webresource["content"]);
        }

        [TestMethod]
        public void ThrowsExceptionWhenWebResourceNotFound()
        {
            
        }
    }
}