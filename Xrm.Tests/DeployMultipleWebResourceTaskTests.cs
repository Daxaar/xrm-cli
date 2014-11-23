using System;
using System.Linq;
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
    public class DeployMultipleWebResourceTaskTests
    {
        private static ILog CreateLog(string promptResponse = "yes")
        {
            var log = new Mock<ILog>();
            log.Setup(x => x.Prompt(It.IsAny<string>())).Returns(promptResponse);
            return log.Object;
        }

        [TestMethod]
        public void DeploysAllFilesInFolder()
        {
            var args = new[] { "deploy", "c:\\xyz", "conn:connectionName" };
            var reader  = new MockFileReaderBuilder().Returns(3).ModifiedFiles.WithRandomFileContent.Build();

            var task = new DeployMultipleWebResourceTask(new DeployWebResourceCommandLine(args), reader.Object);
            var context = new Mock<IXrmTaskContext>();
            var service = new Mock<IOrganizationService>();
            var collectionWithOneRecord = new EntityCollection(new[] { new Entity("webresource") });

            service.Setup(x => x.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(collectionWithOneRecord);
            context.Setup(x => x.ServiceFactory.Create(It.IsAny<string>())).Returns(service.Object);
            context.Setup(x => x.Log).Returns(CreateLog());
            context.Setup(x => x.Configuration).Returns(new StubXrmConfiguration());

            task.Execute(context.Object);
            service.Verify(x=>x.Update(It.IsAny<Entity>()),Times.Exactly(3));
        }

        [TestMethod]
        public void DoesNotDeployFilesWhenConfirmIsNo()
        {
            var args = new[] { "deploy", "c:\\xyz", "conn:connectionName" };

            var task = new DeployMultipleWebResourceTask(new DeployWebResourceCommandLine(args), new MockFileReaderBuilder().Build().Object);
            var context = new Mock<IXrmTaskContext>();
            var service = new Mock<IOrganizationService>();
            context.Setup(x => x.ServiceFactory.Create(It.IsAny<string>())).Returns(service.Object);
            context.Setup(x => x.Log).Returns(CreateLog("no"));
            context.Setup(x => x.Configuration).Returns(new StubXrmConfiguration());

            task.Execute(context.Object);
            service.Verify(x => x.Update(It.IsAny<Entity>()), Times.Never);
        }

        [TestMethod]
        public void DoesNotAskForConfirmationWhenNoConfirmOptionSet()
        {
            var args = new[] { "deploy", "c:\\xyz", "--noconfirm", "conn:connectionName" };
            var task = new DeployMultipleWebResourceTask(new DeployWebResourceCommandLine(args), new Mock<IFileReader>().Object);
            var context = new Mock<IXrmTaskContext>();
            context.Setup(x => x.Configuration).Returns(new StubXrmConfiguration());

            var log     = new Mock<ILog>();

            task.Execute(context.Object);
            log.Verify(x => x.Prompt(It.IsAny<string>()), Times.Never);
        }
    }
}