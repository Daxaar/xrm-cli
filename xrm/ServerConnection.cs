using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Xrm
{
    public class ServerConnection
    {
        public ServerConnection(List<string> spaceDelimitedArgs)
        {
            ServerName = ReadArg(spaceDelimitedArgs, "server") ?? ReadArg(spaceDelimitedArgs, "s") ?? "localhost";

            OrganizationName = ReadArg(spaceDelimitedArgs, "org") ?? ReadArg(spaceDelimitedArgs, "o");
            if (string.IsNullOrEmpty(OrganizationName))
            {
                throw new ArgumentException("Organization name is required format is o:yourorgname");
            }

            Port = ReadArg(spaceDelimitedArgs, "port") ?? ReadArg(spaceDelimitedArgs, "p") ?? "80";
            Protocol = ReadArg(spaceDelimitedArgs, "protocol") ?? "http";
        }

        private string ReadArg(IEnumerable<string> args, string argName )
        {
            var arg = args.FirstOrDefault(x => x.Trim().StartsWith(argName + ":"));
            if (!string.IsNullOrEmpty(arg))
            {
                arg = arg.Trim();
                arg = arg.Substring(arg.IndexOf(":") + 1);
            }

            return arg;
        }

        public string ServerName { get; set; }
        public string OrganizationName { get; set; }
        public string Port { get; set; }
        public string Protocol { get; set; }

        public IOrganizationService CreateOrgService()
        {
            var creds = new ClientCredentials();

            creds.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;

            var uri = new Uri(string.Format("{0}://{1}:{2}/{3}/XRMServices/2011/Organization.svc",Protocol, ServerName,Port,OrganizationName));
            Console.WriteLine("Connecting...");
            var proxy = new OrganizationServiceProxy(uri, null, creds, null);
            proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.OpenTimeout = new TimeSpan(0, 0, 5, 0);
            proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.ReceiveTimeout = new TimeSpan(0, 0, 5, 0);
            proxy.EnableProxyTypes();
            return proxy;
        }
    }
}