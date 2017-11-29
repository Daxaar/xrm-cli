using Octono.Xrm.Tasks;

namespace Octono.Xrm.ConsoleTaskRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Uncomment for testing via console.
            //args = new []{"pull","new_/app.js","to:app.js","w12dev"};
            var runner = new XrmTaskRunner(new ConsoleLogger());
            
            runner.Run(args);
        }
    }
}
