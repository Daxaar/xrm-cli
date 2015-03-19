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
            if (ShowHelp(context.Log)) return;

            if(_args.Count < 3)
                throw new InvalidOperationException("You must specify at least a name for the connection and the Organisation name.");

            var uri = new Uri(_args[1]);

            if (uri.Segments.Count() != 2)
                throw new FormatException("The URL must be in the format scheme://server/org");

            var user = _args.FirstOrDefault(a => a.StartsWith("user:"));
            if (string.IsNullOrEmpty(user) == false)
            {
                user = user.Remove(0, 5);
            }

            //TODO: REMOVE THIS HIDEOUS HACK ADDED FOR A QUICK FIX LOCALLY
            var pwd = _args.FirstOrDefault(a => a.StartsWith("pwd:"));
            if (string.IsNullOrEmpty(pwd) == false)
            {
                pwd = pwd.Remove(0, 4);
            }
            var connectionInfo = new ConnectionInfo
                {
                    ServerName = uri.Host,
                    Port = uri.Port,
                    Protocol = uri.Scheme,
                    Organisation = uri.Segments.Last(),
                    Name = _args.Last(),
                    UserName = user,
                    Password = pwd
                };

            context.Configuration.ConnectionStrings[connectionInfo.Name] = connectionInfo;
        }

        private bool ShowHelp(ILog log)
        {
            if (_args.Any(arg => arg.Contains("help")))
            {
                log.Write("\nUsage");
                log.Write(@"AddConnection ConnectionName http://server:port/org\n");
                return true;
            }
            return false;
        }
    }
}