using Octono.Xrm.Tasks;

namespace Octono.Xrm.ConsoleTaskRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //args = new[]
            //    {
            //        "export",
            //        "IQDevelopment",
            //        "o:prosper",
            //        "s:dc1",
            //        "p:5555",
            //        "--save"
            //    };

            var runner = new XrmTaskRunner(new ConsoleLogger());
            runner.Run(args);
        }
    }
}
