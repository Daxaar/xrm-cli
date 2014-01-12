using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;

namespace Xrm.Tests
{
    [TestClass]
    public class SolutionImportTaskTests
    {
        [TestMethod]
        public void ExecutesImportRequestWhenCommandIsValid()
        {
            var service = new Mock<IOrganizationService>();
            var reader = new Mock<IFileReader>();
            var command = new SolutionImportCommandLine(reader.Object)
            {
                SolutionFilePath = @"c:\path\to\file.zip"
            };
            IXrmTask task = new SolutionImportTask(command, service.Object);
            
            reader.Setup(x => x.ReadAllBytes(command.SolutionFilePath)).Returns(new byte[] { });
            task.Execute();
            service.Verify(x => x.Execute(It.IsAny<ImportSolutionRequest>()),Times.Once());
            reader.Verify(x => x.ReadAllBytes(command.SolutionFilePath), Times.Once());
        }
    }


    public class SystemFileReader : IFileReader
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
