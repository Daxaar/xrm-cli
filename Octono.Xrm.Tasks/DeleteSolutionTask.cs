using System.Linq;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Octono.Xrm.Tasks
{
    public class DeleteSolutionTask : XrmTask
    {
        private readonly DeleteSolutionCommandLine _command;

        public DeleteSolutionTask(DeleteSolutionCommandLine command)
        {
            _command = command;
        }

        public override void Execute(IXrmTaskContext context)
        {
            ShowHelp(context.Log);
            IOrganizationService service = context.ServiceFactory.Create(_command.ConnectionName);
            using (var ctx = new OrganizationServiceContext(service))
            {
                var solution = from s in ctx.CreateQuery("solution")
                               where s.GetAttributeValue<string>("uniquename") == _command.SolutionName
                               select s.Id;

                context.Log.Write(string.Format("Deleting solution {0}", _command.SolutionName));
                service.Delete("solution",solution.Single());
                service.Execute(new PublishAllXmlRequest());
                context.Log.Write(string.Format("Solution deleted successfully"));
            }
        }

        private void ShowHelp(ILog log)
        {
            if (!_command.ShowHelp) return;
            
            log.Write("Deletes a solution from the target organisation");
            log.Write("Usage");
            log.Write(@"delete solution solutionname");
            log.Write("[solutionname] parameter needs to be the unique name and not display name");
        }
    }
}