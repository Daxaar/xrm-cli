using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;
using Octono.Xrm.Tests.Builders;

namespace Octono.Xrm.Tests
{
    [TestClass]
    public class DeployWebResourceTaskTests
    {
        private const string Path = @"c:\test\ntt_contribution.js";
        private const string WebResourceContent = "random javascript content";
        private static readonly string[] Args = new[] { "deploy", Path };

        [TestMethod]
        public void DoesNotUpdateWebResourceWhenLocalFileIsEmptyAndForceFlagNotSpecified()
        {
            var task = new DeployWebResourceTask(new DeployWebResourceCommandLine(Args), new Mock<IFileReader>().Object, MockConfigurationManagerBuilder.Build());
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
            var reader  = new MockFileReaderBuilder().Returns(3).ModifiedFiles.WithRandomFileContent.Build();
            var task = new DeployWebResourceTask(new DeployWebResourceCommandLine(Args), reader.Object, MockConfigurationManagerBuilder.Build());
            var context = new Mock<IXrmTaskContext>();
            var service = new Mock<IOrganizationService>();
            var collectionWithOneRecord = new EntityCollection(new[] { new Entity("webresource") });

            service.Setup(x => x.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(collectionWithOneRecord);
            context.Setup(x => x.Service).Returns(service.Object);
            context.Setup(x => x.Log).Returns(new Mock<ILog>().Object);
            
            task.Execute(context.Object);
            
            reader.Verify(x=>x.ReadAllBytes(Path),Times.Once);
        }

        [TestMethod]
        public void LoadsWebResourceFromServer()
        {
            var context         = new Mock<IXrmTaskContext>();
            var service         = new Mock<IOrganizationService>();
            var collectionWithOneRecord    = new EntityCollection(new[] { new Entity("webresource") });
            var task = new DeployWebResourceTask(new DeployWebResourceCommandLine(Args), CreateFileReaderWithContent().Object, MockConfigurationManagerBuilder.Build());

            service.Setup(x => x.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(collectionWithOneRecord);
            context.Setup(x => x.Service).Returns(service.Object);
            context.Setup(x => x.Log).Returns(new Mock<ILog>().Object);

            task.Execute(context.Object);
            
            service.Verify(x=>x.RetrieveMultiple(It.IsAny<QueryBase>()),Times.Once);
        }

        private static Mock<IFileReader> CreateFileReaderWithContent()
        {
            var reader = new Mock<IFileReader>();
            reader.Setup(x => x.ReadAllBytes(Path)).Returns(Encoding.UTF8.GetBytes(WebResourceContent));
            return reader;
        }

        [TestMethod]
        public void UpdatesWebResourceContentAsBase64String()
        {
            var context = new Mock<IXrmTaskContext>();
            var service = new Mock<IOrganizationService>();
            var webresource = new Entity("webresource") { Attributes = new AttributeCollection { { "name", "ntt_contribution" } } };

            var task = new DeployWebResourceTask(new DeployWebResourceCommandLine(Args), CreateFileReaderWithContent().Object, MockConfigurationManagerBuilder.Build());

            service.Setup(x => x.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(new EntityCollection(new[] { webresource } ));
            context.Setup(x => x.Service).Returns(service.Object);
            context.Setup(x => x.Log).Returns(new Mock<ILog>().Object);

            task.Execute(context.Object);

            service.Verify(x => x.RetrieveMultiple(It.IsAny<QueryBase>()), Times.Once);
            service.Verify(x => x.Update(webresource),Times.Once);
            Assert.AreEqual(WebResourceContent.ToBase64String(),webresource["content"]);
        }
    }
}