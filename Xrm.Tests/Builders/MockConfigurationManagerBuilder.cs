using System.Collections.Specialized;
using Moq;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tests.Builders
{
    public static class MockConfigurationManagerBuilder
    {
        public static IConfigurationManager Build()
        {
            var cm = new Mock<IConfigurationManager>();
            cm.Setup(x => x.AppSettings).Returns(new NameValueCollection());
            return new Mock<IConfigurationManager>().Object;
        }
    }
}