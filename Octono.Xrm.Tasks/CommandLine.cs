using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octono.Xrm.Tasks
{
    public abstract class CommandLine
    {
        protected CommandLine(IList<string> args)
        {
            ConnectionName = args.SingleOrDefault(x => x.StartsWith("cn:") || x.StartsWith("conn:"));

            if (string.IsNullOrEmpty(ConnectionName))
            {
                throw new ArgumentException("Connection name [cn:name or conn:name] argument has not been specified.");
            }
            ConnectionName = ConnectionName.Substring(ConnectionName.IndexOf(":", System.StringComparison.Ordinal) + 1);
            Args = args;
        }
        public string ConnectionName { get; set; }
        public IList<string> Args { get; private set; }
    }
}
