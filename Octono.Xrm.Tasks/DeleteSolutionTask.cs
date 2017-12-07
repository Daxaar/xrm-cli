using System;
using System.Linq;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Deletes a solution from an Organisation
    /// </summary>
    public class DeleteSolutionTask : IXrmTask
    {
        private readonly DeleteSolutionCommandLine _command;

        public DeleteSolutionTask(DeleteSolutionCommandLine command)
        {
            _command = command;
        }

        public void Execute(IXrmTaskContext context)
        {
            ShowHelp(context.Log);
            IOrganizationService service = context.ServiceFactory.Create(_command.ConnectionName);
            using (var ctx = new OrganizationServiceContext(service))
            {
                Console.WriteLine($"Are you sure you want to delete the solution {_command.SolutionName}?");
                var answer = Console.ReadLine();

                if (answer != "y" || answer != "yes")
                {
                    return;
                }
                var solution = from s in ctx.CreateQuery("solution")
                    where s.GetAttributeValue<string>("uniquename") == _command.SolutionName
                    select s.Id;

                context.Log.Write($"Deleting solution {_command.SolutionName}");
                service.Delete("solution", solution.Single());
                service.Execute(new PublishAllXmlRequest());
                context.Log.Write("Solution deleted successfully");
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