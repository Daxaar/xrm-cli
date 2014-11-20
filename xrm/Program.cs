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
            //        "name:development",
            //        "--save"
            //    };

            //args = new[]
            //    {
            //        "export",
            //        "BaseSolution",
            //        "o:org",
            //        "s:localhost",
            //        "name:dfe"
            //    };

            //args = new[]{"addconnection","o:XRMDEV2","s:localhost","conn:xrmdev-local2"};
            //args = new[]{"deploy",@"ntt_contribution.js","conn:xrmdev-local"};

            var runner = new XrmTaskRunner(new ConsoleLogger());
            
            runner.Run(args);
        }
    }
}
