using System.Collections.Generic;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tests.Builders;
using Xunit;

namespace Octono.Xrm.Tests
{
    public class AddConnectionTaskTests
    {
        [Fact]
        public void AcceptsConnectionNameAsLastParameter()
        {
            var task = new AddConnectionTask(new AddConnectionCommandLine(new List<string> { "addconnection", "http://server/org", "connectionname" }));
            var context = new MockXrmTaskContext();
            task.Execute(context.Object);
            Assert.True(context.Configuration.ConnectionStrings.ContainsKey("connectionname"));
        }
    }
}