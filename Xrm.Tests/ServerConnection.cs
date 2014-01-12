using System;
using System.Collections.Generic;
using System.Linq;

namespace Xrm.Tests
{
    public class ServerConnection
    {
        public ServerConnection(List<string> spaceDelimitedArgs)
        {
            ServerName = ReadArg(spaceDelimitedArgs, "server") ?? ReadArg(spaceDelimitedArgs, "s") ?? "localhost";

            OrganizationName = ReadArg(spaceDelimitedArgs, "org") ?? ReadArg(spaceDelimitedArgs, "o");
            if (string.IsNullOrEmpty(OrganizationName))
            {
                throw new ArgumentException("Organization name is required format is o:yourorgname");
            }

            Port = ReadArg(spaceDelimitedArgs, "port") ?? "80";

            Protocol = ReadArg(spaceDelimitedArgs, "protocol") ?? "http";
        }

        private string ReadArg(IEnumerable<string> args, string argName )
        {
            var arg = args.FirstOrDefault(x => x.Trim().StartsWith(argName + ":"));
            if (!string.IsNullOrEmpty(arg))
            {
                arg = arg.Trim();
                arg = arg.Substring(arg.IndexOf(":") + 1);
            }

            return arg;
        }

        public string ServerName { get; set; }
        public string OrganizationName { get; set; }
        public string Port { get; set; }
        public string Protocol { get; set; }
    }
}