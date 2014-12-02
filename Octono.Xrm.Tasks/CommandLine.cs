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
            //Read the connection if the user has explicitly set it in the params with the prefix otherwise
            //assume the convention where it's always the last param
            ConnectionName = args.SingleOrDefault(x => x.StartsWith("cn:") || x.StartsWith("conn:")) ??
                             args.LastOrDefault();
            Args = args;
        }
        public string ConnectionName { get; set; }
        public IList<string> Args { get; private set; }
    }
}
