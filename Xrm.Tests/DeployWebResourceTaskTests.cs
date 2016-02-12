using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;
using Octono.Xrm.Tests.Builders;
using Xunit;

//TODO: Factory method for context

namespace Octono.Xrm.Tests
{
    
    public class DeployWebResourceTaskTests
    {
        private const string Path = @"c:\test\resourcename.js";
        private static readonly string[] Args = new[] { "deploy", Path, "conn:connectionName" };
        private readonly Mock<IWebResourceQuery> _query;

        public DeployWebResourceTaskTests()
        {
            var resource = new Entity();
            resource.Attributes.Add("content","somerandomcontent");

            _query = new Mock<IWebResourceQuery>();
            _query.Setup(x => x.Retrieve(It.IsAny<IOrganizationService>(), It.IsAny<string>()))
                  .Returns(resource);
        }

        [Fact]
        public void UsesNameArgumentWhenSpecified()
        {
            const string expectedWebResourceName = "new_resourcename";
            var args = Args.ToList();
            var reader = new MockFileReaderBuilder().Returns(1).ModifiedFiles.WithRandomFileContent;
            args.Add("name:" + expectedWebResourceName);
            var meta = new Mock<IWebResourceMetaData>();

            var task = new DeployWebResourceTask(new DeployWebResourceCommandLine(args), reader.Build().Object,_query.Object,meta.Object);
            var context = new MockXrmTaskContext();
            task.Execute(context.Object);
            _query.Verify(x => x.Retrieve(It.IsAny<IOrganizationService>(), expectedWebResourceName), Times.Once);
        }
        
        [Fact]
        public void DoesNotUpdateWebResourceWhenLocalFileIsEmptyAndForceFlagNotSpecified()
        {
            var meta = new Mock<IWebResourceMetaData>();
            var task = new DeployWebResourceTask(new DeployWebResourceCommandLine(Args), new Mock<IFileReader>().Object, _query.Object,meta.Object);
            var context = new MockXrmTaskContext();
            
            task.Execute(context.Object);
            context.Service.Verify(x=>x.Update(It.IsAny<Entity>()),Times.Never);
        }

        [Fact]
        public void ReadsWebResourceFileFromDisk()
        {
            var meta = new Mock<IWebResourceMetaData>();
            var reader  = new MockFileReaderBuilder().Returns(3).ModifiedFiles.WithRandomFileContent.Build();
            var task = new DeployWebResourceTask(new DeployWebResourceCommandLine(Args), reader.Object, _query.Object,meta.Object);
            var context = new MockXrmTaskContext();
            
            var collectionWithOneRecord = new EntityCollection(new[] { new Entity("webresource") });

            context.Service.Setup(x => x.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(collectionWithOneRecord);
            task.Execute(context.Object);
            
            reader.Verify(x=>x.ReadAllBytes(Path),Times.Once);
        }
    }
}