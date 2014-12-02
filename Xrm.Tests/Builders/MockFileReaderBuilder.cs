using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Moq;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tests.Builders
{
    internal class MockXrmTaskContext : Mock<IXrmTaskContext>
    {
        public MockXrmTaskContext()
        {
            Service = new Mock<IOrganizationService>();
            Log = new Mock<ILog>();
            Configuration = new StubXrmConfiguration();

            Setup(x => x.ServiceFactory.Create(It.IsAny<string>())).Returns(Service.Object);
            Setup(x => x.Log).Returns(new Mock<ILog>().Object);
            Setup(x => x.Configuration).Returns(Configuration);
        }

        public Mock<ILog> Log { get; set; }
        public Mock<IOrganizationService> Service { get; set; }

        public StubXrmConfiguration Configuration { get; set; }
    }

    internal class MockFileReaderBuilder
    {
        private readonly Mock<IFileReader> _reader = new Mock<IFileReader>();
        private int _currentCount;
        public MockFileReaderBuilder Returns(int count)
        {
            _currentCount = count;
            return this;
        }

        public MockFileReaderBuilder ModifiedFiles
        {
            get
            {
                var files = new List<string>();
                for (int i = 0; i < _currentCount; i++)
                {
                    files.Add(string.Format("file{0}.js", i));
                }

                _reader.Setup(x => x.GetModifiedFilesSince(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>()))
                       .Returns(files);

                _currentCount = 0;
                return this;                
            }
        }

        public MockFileReaderBuilder WithRandomFileContent
        {
            get
            {
                _reader.Setup(x => x.ReadAllBytes(It.IsAny<string>())).Returns(() => Guid.NewGuid().ToByteArray());
                return this;                
            }
        }
        public MockFileReaderBuilder WithEmptyFileContent
        {
            get
            {
                _reader.Setup(x => x.ReadAllBytes(It.IsAny<string>())).Returns(new byte[] { });
                return this;                
            }
        }

        public Mock<IFileReader> Build()
        {
            return _reader ?? new Mock<IFileReader>();
        }
    }
}