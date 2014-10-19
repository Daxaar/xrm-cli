using Octono.Xrm.Tasks;

namespace Octono.Xrm.ConsoleTaskRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //args = new[]
            //    {
            //        "Enter args here for testing"
            //    };

            var runner = new XrmTaskRunner(new ConsoleLogger());
            runner.Run(args);
        }
    }
}
