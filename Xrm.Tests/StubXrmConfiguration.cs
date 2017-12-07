using System.Collections.Generic;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tests
{
    public class StubXrmConfiguration : IXrmConfiguration
    {
        public StubXrmConfiguration()
        {
            AppSettings = new Dictionary<string, string>();
            ConnectionStrings = new Dictionary<string, ConnectionInfo>();
        }

        public Dictionary<string, string> AppSettings { get; }
        public Dictionary<string, ConnectionInfo> ConnectionStrings { get; }
    }
}