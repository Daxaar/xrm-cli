using Newtonsoft.Json;
using System.Security;

namespace Octono.Xrm.Tasks.IO
{
    public class ConnectionInfo
    {
        [JsonIgnore]
        public string Name { get; set; }
        public string ServerName { get; set; }
        public string Organisation { get; set; }
        public int Port { get; set; }
        public string Protocol { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public bool Debug { get; set; }

        [JsonIgnore]
        public string ConnectionString
        {
            get
            {
                return string.Format("{0}://{1}:{2}/{3}/XRMServices/2011/Organization.svc", Protocol, ServerName, Port,
                                     Organisation);
            }
        }
    }
}