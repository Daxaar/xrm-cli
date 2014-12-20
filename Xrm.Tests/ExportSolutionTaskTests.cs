using System;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Moq;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;
using Octono.Xrm.Tests.Builders;
using Xunit;

namespace Octono.Xrm.Tests
{
    
    public class ExportSolutionTaskTests
    {
        [Fact]
        public void ExportsAllSolutionsProvidedOnCommandLine()
        {
            var command = new ExportSolutionCommandLine(new[] { "export", "sol1,sol2", "connectionName" });
            var writer = new Mock<IFileWriter>();
            var context = new MockXrmTaskContext();
            context.Service.Setup(x => x.Execute(It.IsAny<OrganizationRequest>())).Returns(new ExportSolutionResponse());

            var task = new ExportSolutionTask(command, writer.Object);
            
            writer.Setup(x => x.Write(It.IsAny<byte[]>(), It.IsAny<string>())).Callback<byte[], string>((x, y) => Console.WriteLine(y));
            
            task.Execute(context.Object);
            
            writer.Verify(x=>x.Write(It.IsAny<byte[]>(),It.IsAny<string>()),Times.Exactly(2));
        }
    }
}
