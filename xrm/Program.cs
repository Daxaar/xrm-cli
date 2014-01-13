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
                    @"Z:\Dropbox\UKMail\src\crm-solutions\approvalframework13-01-14-09-18-55.zip",
                    "org:prosper",
                    "port:5555"
                };

            var connection = new ServerConnection(args.ToList());
            var taskFactory = new XrmTaskFactory(args, new SystemFileReader(), connection.CreateOrgService());
            var runner = new XrmTaskRunner(taskFactory, new ConsoleLogger());

            runner.Run();
            return;

            //var creds = new ClientCredentials();

            //creds.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;

            //var uri = new Uri(string.Format("http://{0}:{1}/{2}/XRMServices/2011/Organization.svc",server,port,org));
            //Console.WriteLine("Connecting...");
            //using (var proxy = new OrganizationServiceProxy(uri, null, creds, null))
        }
    }
}
