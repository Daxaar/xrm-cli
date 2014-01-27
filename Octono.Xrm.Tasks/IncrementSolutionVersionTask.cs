using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Octono.Xrm.Tasks
{
    public class IncrementSolutionVersionTask : IXrmTask
    {
        private readonly string _solutionName;
        private readonly IOrganizationService _service;
        private readonly ILog _log;

        public IncrementSolutionVersionTask(string solutionName, IOrganizationService service, ILog log)
        {
            _solutionName = solutionName;
            _service = service;
            _log = log;
        }

        public void Execute()
        {
            _log.Write(string.Format("Incrementing Version Number for solution {0}",_solutionName));
            string oldVersion, newVersion;

            using (var context = new OrganizationServiceContext(_service))
            {
                var solution = context.CreateQuery("solution")
                                      .First(s => s.GetAttributeValue<string>("uniquename") == _solutionName);

                oldVersion = solution.GetAttributeValue<string>("version");

                var formatter = new SolutionVersionFormatter();
                newVersion = formatter.Increment(oldVersion);
                solution["version"] = newVersion;
                context.UpdateObject(solution);
                context.SaveChanges();
            }
            _log.Write(string.Format("Incremented Solution {0} from {1} to {2}",_solutionName,oldVersion,newVersion));
        }
    }
    public class IncrementSolutionCommandLine
    {
        public string Solution { get; set; }
    }
}
