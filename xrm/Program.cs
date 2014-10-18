using Octono.Xrm.Tasks;

namespace Octono.Xrm.ConsoleTaskRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            args = new[]
                {
                    "deploy",
                    "c:\\test\\ntt_contribution.js",
                    "o:xrmdev",
                    "s:w12dev"
                };

            var runner = new XrmTaskRunner(new ConsoleLogger());
            runner.Run(args);
        }
    }
}
