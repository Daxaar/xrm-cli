using System.Linq;
using Microsoft.Xrm.Sdk.Client;

namespace Octono.Xrm.Tasks
{
    public class IncrementSolutionVersionTask : IXrmTask
    {
        private readonly string _solutionName;

        public IncrementSolutionVersionTask(string solutionName)
        {
            _solutionName = solutionName;
        }

        public void Execute(IXrmTaskContext context)
        {
            context.Log.Write(string.Format("Incrementing Version Number for solution {0}",_solutionName));
            string oldVersion, newVersion;

            using (var orgContext = new OrganizationServiceContext(context.Service))
            {
                var solution = orgContext.CreateQuery("solution")
                                      .First(s => s.GetAttributeValue<string>("uniquename") == _solutionName);

                oldVersion = solution.GetAttributeValue<string>("version");

                var formatter = new SolutionVersionFormatter();
                newVersion = formatter.Increment(oldVersion);
                solution["version"] = newVersion;
                orgContext.UpdateObject(solution);
                orgContext.SaveChanges();
            }
            context.Log.Write(string.Format("Incremented Solution {0} from {1} to {2}",_solutionName,oldVersion,newVersion));
        }
        public bool RequiresServerConnection { get { return true; } }
    }
    public class IncrementSolutionCommandLine
    {
        public string Solution { get; set; }
    }

}
