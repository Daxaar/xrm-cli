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
            var context = new MockXrmTaskContext();

            var collectionWithOneRecord = new EntityCollection(new[] { new Entity("webresource") });
            context.Setup(x => x.Log).Returns(CreateLog("yes"));
            context.Service.Setup(x => x.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(collectionWithOneRecord);
            task.Execute(context.Object);
            context.Service.Verify(x=>x.Update(It.IsAny<Entity>()),Times.Exactly(3));
        }

        [TestMethod]
        public void DoesNotDeployFilesWhenConfirmIsNo()
        {
            var args = new[] { "deploy", "c:\\xyz", "connectionName" };

            var task = new DeployMultipleWebResourceTask(new DeployWebResourceCommandLine(args), new MockFileReaderBuilder().Build().Object);
            var context = new MockXrmTaskContext();
            context.Setup(x => x.Log).Returns(CreateLog("no"));
            task.Execute(context.Object);
            context.Service.Verify(x => x.Update(It.IsAny<Entity>()), Times.Never);
        }

        [TestMethod]
        public void DoesNotAskForConfirmationWhenNoConfirmOptionSet()
        {
            var args = new[] { "deploy", "c:\\xyz", "--noconfirm", "connectionName" };
            var task = new DeployMultipleWebResourceTask(new DeployWebResourceCommandLine(args), new Mock<IFileReader>().Object);
            var context = new MockXrmTaskContext();

            var log     = new Mock<ILog>();

            task.Execute(context.Object);
            log.Verify(x => x.Prompt(It.IsAny<string>()), Times.Never);
        }
    }
}