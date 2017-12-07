using Microsoft.Xrm.Sdk.Query;
using Octono.Xrm.Tasks.Utils;

namespace Octono.Xrm.Tasks
{
    [XrmTask(typeof(ConnectionTestCommandLine),"testconnection",Aliases = new []{"testconn","contest"})]
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
}