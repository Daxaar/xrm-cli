

using Octono.Xrm.ConsoleTaskRunner;
using Octono.Xrm.Tasks;

namespace xrm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var runner = new XrmTaskRunner(new ConsoleLogger());
            runner.Run(args);
        }
    }
}
