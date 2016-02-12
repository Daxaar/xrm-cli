using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;
using Octono.Xrm.Tests.Builders;
using Xunit;


namespace Octono.Xrm.Tests
{
    public class DeployMultipleWebResourceTaskTests
    {
        private static ILog CreateLog(string promptResponse = "yes")
        {
            var log = new Mock<ILog>();
            log.Setup(x => x.Prompt(It.IsAny<string>())).Returns(promptResponse);
            return log.Object;
        }

        [Fact]
        public void DeploysAllFilesInFolder()
        {
            var args = new[] { "deploy", "c:\\xyz", "conn:connectionName" };
            var reader  = new MockFileReaderBuilder().Returns(3).ModifiedFiles.WithRandomFileContent.Build();
            var writer = new Mock<IFileWriter>();
            var task = new DeployMultipleWebResourceTask(new DeployWebResourceCommandLine(args), reader.Object,writer.Object);
            var context = new MockXrmTaskContext();

            var collectionWithOneRecord = new EntityCollection(new[] { new Entity("webresource") });
            context.Setup(x => x.Log).Returns(CreateLog());
            context.Service.Setup(x => x.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(collectionWithOneRecord);
            task.Execute(context.Object);
            context.Service.Verify(x=>x.Update(It.IsAny<Entity>()),Times.Exactly(3));
        }

        [Fact]
        public void DoesNotDeployFilesWhenConfirmIsNo()
        {
            var args = new[] { "deploy", "c:\\xyz", "connectionName" };
            var writer = new Mock<IFileWriter>();
            var task = new DeployMultipleWebResourceTask(new DeployWebResourceCommandLine(args), new MockFileReaderBuilder().Build().Object,writer.Object);
            var context = new MockXrmTaskContext();
            context.Setup(x => x.Log).Returns(CreateLog("no"));
            task.Execute(context.Object);
            context.Service.Verify(x => x.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void DoesNotAskForConfirmationWhenNoConfirmOptionSet()
        {
            var args = new[] { "deploy", "c:\\xyz", "--noconfirm", "connectionName" };
            var writer = new Mock<IFileWriter>();
            var task = new DeployMultipleWebResourceTask(new DeployWebResourceCommandLine(args), new Mock<IFileReader>().Object,writer.Object);
            var context = new MockXrmTaskContext();

            var log     = new Mock<ILog>();

            task.Execute(context.Object);
            log.Verify(x => x.Prompt(It.IsAny<string>()), Times.Never);
        }
    }
}