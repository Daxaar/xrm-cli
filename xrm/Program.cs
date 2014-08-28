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
            //        "Solution1,Solution2",
            //        "o:XRM2DEV",
            //        "--save"
            //    };

            var runner = new XrmTaskRunner(new ConsoleLogger());
            runner.Run(args);
        }
    }
}
