using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Increments the version number of a solution using the semantic versioning format http://semver.org/
    /// </summary>
    public class IncrementSolutionVersionTask : IXrmTask
    {
        private readonly IncrementSolutionCommandLine _commandLine;

        public IncrementSolutionVersionTask(IncrementSolutionCommandLine commandLine)
        {
            _commandLine = commandLine;
        }

        public void Execute(IXrmTaskContext context)
        {
            context.Log.Write($"Incrementing Version Number for solution {_commandLine.Solution}");
            string oldVersion, newVersion;

            IOrganizationService service = context.ServiceFactory.Create(_commandLine.ConnectionName);
            using (var orgContext = new OrganizationServiceContext(service))
            {
                var solution = orgContext.CreateQuery("solution")
                                      .First(s => s.GetAttributeValue<string>("uniquename") == _commandLine.Solution);

                oldVersion = solution.GetAttributeValue<string>("version");

                var formatter = new SolutionVersionFormatter();
                newVersion = formatter.Increment(oldVersion);
                solution["version"] = newVersion;
                orgContext.UpdateObject(solution);
                orgContext.SaveChanges();
            }
            context.Log.Write($"Incremented Solution {_commandLine.Solution} from {oldVersion} to {newVersion}");
        }
    }
    public class IncrementSolutionCommandLine : CommandLine
    {
        public IncrementSolutionCommandLine(IList<string> args) : base(args)
        {
            Solution = args.FirstOrDefault(arg => arg.StartsWith("s:"));
        }

        public string Solution { get; set; }
    }

}
