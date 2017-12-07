using System.Collections.Generic;

namespace Octono.Xrm.Tasks.IO
{
    public interface IXrmConfiguration
    {
        Dictionary<string, string> AppSettings { get; }
        Dictionary<string, ConnectionInfo> ConnectionStrings { get; }
    }
}