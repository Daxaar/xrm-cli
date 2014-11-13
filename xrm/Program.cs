using Octono.Xrm.Tasks;

namespace Octono.Xrm.ConsoleTaskRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Uncomment for testing via console.
            //args = new[]
            //    {
            //        "export",
            //        "solutionname",
            //        "o:orgname",
            //        "s:servername",
            //        "p:5555",
            //        "protocol:http",
            //        "name:server:orgname",
            //        "--save"
            //    };

            var runner = new XrmTaskRunner(new ConsoleLogger());
            runner.Run(args);
        }
    }
}
