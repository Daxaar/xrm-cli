using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    public class ServerConnection : IDisposable
    {
        private readonly ILog _logger;
        private readonly IConfigurationManager _config;
        OrganizationServiceProxy _proxy;

        public string ServerName { get; set; }
        public string Organisation { get; set; }
        public string Port { get; set; }
        public string Protocol { get; set; }
        public bool Debug { get; set; }

        public bool Save { get; set; }

        public ServerConnection(IList<string> spaceDelimitedArgs, ILog logger,IConfigurationManager config)
        {
            _logger     = logger;
            _config = config;
            
            Debug = spaceDelimitedArgs.Contains("--debug");
            Save = spaceDelimitedArgs.Contains("--save");

            ServerName = ReadArg(spaceDelimitedArgs, "server") ?? 
                         ReadArg(spaceDelimitedArgs, "s") ?? 
                         ReadFromConfig("server","localhost");
            
            Port =  ReadArg(spaceDelimitedArgs, "port") ?? 
                    ReadArg(spaceDelimitedArgs, "p") ?? 
                    ReadFromConfig("port","80");

            Protocol = ReadArg(spaceDelimitedArgs, "protocol") ?? ReadFromConfig("protocol", "http");

            Organisation =  ReadArg(spaceDelimitedArgs, "org") ?? 
                            ReadArg(spaceDelimitedArgs, "o") ?? 
                            ReadFromConfig("org") ??
                            ThrowOnNull<string>("Organization name parameter is required o:yourorgname");
            
            if (Save)
            {
                config.Add("server", ServerName);
                config.Add("org", Organisation);
                config.Add("port", Port);
                config.Add("protocol", Protocol);
                config.Save();
            }
        }

        private static T ThrowOnNull<T>(string message)
        {
            throw new ArgumentException(message);
        }

        private string ReadFromConfig(string argName, string defaultValue = null)
        {
            //Try and read from config
            string key      = MapArg(argName);
            string value    = _config.AppSettings.AllKeys.Contains(key) ? _config.AppSettings[key] : defaultValue;
            return value;
        }

        private static string MapArg(string name)
        {
            System.Diagnostics.Debug.Assert(name != null);
            
            switch (name)
            {
                case "port":
                case "p":
                    {
                        return "port";
                    }
                case "server":
                case "s":
                    {
                        return "server";
                    }
                case "protocol":
                case "t":
                    {
                        return "protocol";
                    }
                case "org":
                case "o":
                    {
                        return "org";
                    }
            }
            throw new ArgumentException("Cannot map commandline switch", name);
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

        public IOrganizationService CreateOrgService()
        {
            var creds = new ClientCredentials();
            creds.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;

            var uri = new Uri(string.Format("{0}://{1}:{2}/{3}/XRMServices/2011/Organization.svc", Protocol, ServerName, Port, Organisation));
            _logger.Write("Connecting to " + uri.AbsoluteUri);
            
            _proxy = new OrganizationServiceProxy(uri, null, creds, null) {Timeout = new TimeSpan(0, 0, 10, 0)};
            _proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.OpenTimeout = new TimeSpan(0, 0, 10, 0);
            _proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.ReceiveTimeout = new TimeSpan(0, 0, 10, 0);
            _proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.CloseTimeout = new TimeSpan(0, 0, 10, 0);
            _proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.SendTimeout = new TimeSpan(0, 0, 10, 0);
            _proxy.EnableProxyTypes();
            
            return _proxy;
        }

        public void Dispose()
        {
            _proxy.Dispose();
        }
    }
}