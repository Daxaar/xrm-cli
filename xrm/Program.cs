using System.Linq;
using Xrm;

namespace xrm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            args = new[]
                {
                    "import",
                    @"Export\invoicequery.zip",
                    "org:prosper",
                    "port:5555"
                };
            //args = new[]
            //    {
            //        "export",
            //        "invoicequery",
            //        "org:prosper",
            //        "port:5555"
            //    };

            var connection = new ServerConnection(args.ToList());
            var taskFactory = new XrmTaskFactory(new SystemFileReader(), connection.CreateOrgService());
            var task = taskFactory.CreateTask(args);
            task.Execute();
        }
    }
}
