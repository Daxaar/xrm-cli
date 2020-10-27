using Octono.Xrm.Tasks;

namespace Octono.Xrm.ConsoleTaskRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //args = new []{"testconnection","kinderton"};
            var runner = new XrmTaskRunner(new ConsoleLogger());
            runner.Run(args);
        }
    }
}
