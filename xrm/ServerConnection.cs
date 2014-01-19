using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Xrm
{
    public class ServerConnection : IDisposable
    {
        private readonly ILog _logger;
        OrganizationServiceProxy _proxy;

        public ServerConnection(List<string> spaceDelimitedArgs, ILog logger)
        {
            _logger = logger;
            ServerName = ReadArg(spaceDelimitedArgs, "server") ?? ReadArg(spaceDelimitedArgs, "s") ?? "localhost";
            Debug = spaceDelimitedArgs.Contains("--debug");
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
        public bool Debug { get; set; }
        public IOrganizationService CreateOrgService()
        {
            _logger.Write("Connecting...");
            var creds = new ClientCredentials();

            creds.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;

            var uri = new Uri(string.Format("{0}://{1}:{2}/{3}/XRMServices/2011/Organization.svc", Protocol, ServerName, Port, OrganizationName));
            _proxy = new OrganizationServiceProxy(uri, null, creds, null);
            _proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.OpenTimeout = new TimeSpan(0, 0, 5, 0);
            _proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.ReceiveTimeout = new TimeSpan(0, 0, 5, 0);
            _proxy.EnableProxyTypes();
            return _proxy;
        }

        public void Dispose()
        {
            _proxy.Dispose();
        }
    }
}