using System;
using System.Collections.Generic;
using System.Linq;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Adds a CRM Organization Service connection to the application config.json file 
    /// </summary>
    /// <remarks>
    /// The connection is stored with a unique name which can later be used as an argument for any tasks requiring a CRM connection
    /// </remarks>
    public class AddConnectionTask : IXrmTask
    {
        private readonly IList<string> _args;

        public AddConnectionTask(IList<string> args)
        {
            _args = args;
        }

        public void Execute(IXrmTaskContext context)
        {
            var connectionInfo = new ConnectionInfo
                {
                    Debug = _args.Contains("--debug"),
                    ServerName = ReadArg(_args, "server") ?? ReadArg(_args, "s"),
                    Port = Convert.ToInt32(ReadArg(_args, "port") ?? ReadArg(_args, "p") ?? "80"),
                    Protocol = ReadArg(_args, "protocol") ?? "http",
                    Organisation = ReadArg(_args, "org") ?? ReadArg(_args, "o") ??
                                   ThrowOnNull<string>("Organization name parameter is required o:yourorgname"),
                    Name = ReadArg(_args, "name") ??
                           ThrowOnNull<string>("You must specify a connectionInfo name using the c:name_of_connection_in_config_file format.  Use the AddConnection command to add a connection to configuration."),
                };

            context.Configuration.ConnectionStrings[connectionInfo.Name] = connectionInfo;
        }

        private static string ReadArg(IEnumerable<string> args, string argName)
        {
            var arg = args.FirstOrDefault(x => x.Trim().StartsWith(argName + ":"));
            if (!String.IsNullOrEmpty(arg))
            {
                arg = arg.Trim();
                arg = arg.Substring(arg.IndexOf(":", StringComparison.Ordinal) + 1);
            }

            return arg;
        }

        private static T ThrowOnNull<T>(string message)
        {
            throw new ArgumentException(message);
        }
    }
}