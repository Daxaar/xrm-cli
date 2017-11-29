using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Microsoft.Xrm.Sdk.Query;
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
        private readonly AddConnectionCommandLine _commandLine;
        public AddConnectionTask(AddConnectionCommandLine commandLine)
        {
            _commandLine = commandLine;
        }

        public void Execute(IXrmTaskContext context)
        {
            if (ShowHelp(context.Log)) return;

            var uri = _commandLine.Uri;
            var connectionInfo = new ConnectionInfo
                {
                    ServerName = uri.Host,
                    Port = uri.Port,
                    Protocol = uri.Scheme,
                    Organisation = uri.Segments.Last(),
                    Name = _commandLine.Name,
                    UserName = _commandLine.UserName,
                    Password = DapiSecurePassword.Encrypt(_commandLine.Password)
                };

            context.Configuration.ConnectionStrings[connectionInfo.Name] = connectionInfo;
        }

        private bool ShowHelp(ILog log)
        {
            if (_commandLine.ShowHelp)
            {
                log.Write("\nUsage");
                log.Write(@"AddConnection http://server:port/org user:username* pwd:password* connectionName * and optional.  If omitted default logon credentials will be used.\n");
                return true;
            }
            return false;
        }
    }

    public class ConnectionTestTask : IXrmTask
    {
        private readonly ConnectionTestCommandLine _commandLine;

        public ConnectionTestTask(ConnectionTestCommandLine commandLine)
        {
            _commandLine = commandLine;
        }
        public void Execute(IXrmTaskContext context)
        {
            var service = context.ServiceFactory.Create(_commandLine.ConnectionName);

            var q = new QueryExpression
            {
                EntityName = "account",
                ColumnSet = new ColumnSet("name")
            };

            var response = service.RetrieveMultiple(q);

            foreach (var entity in response.Entities)
            {
                context.Log.Write(entity.GetAttributeValue<string>("name"),false);
            }
        }
    }

    public class ConnectionTestCommandLine : CommandLine
    {
        public ConnectionTestCommandLine(IList<string> args) : base(args)
        {
        }
    }
}