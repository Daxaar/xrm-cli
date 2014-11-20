using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    public class OrganizationServiceFactory
    {
        public static IOrganizationService Create(ConnectionInfo connectionInfo, ILog logger)
        {
            var creds = new ClientCredentials();
            creds.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;

            var uri = new Uri(connectionInfo.ConnectionString);
            logger.Write("Connecting to " + uri.AbsoluteUri);

            var proxy = new OrganizationServiceProxy(uri, null, creds, null) { Timeout = new TimeSpan(0, 0, 10, 0) };
            proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.OpenTimeout = new TimeSpan(0, 0, 10, 0);
            proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.ReceiveTimeout = new TimeSpan(0, 0, 10, 0);
            proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.CloseTimeout = new TimeSpan(0, 0, 10, 0);
            proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.SendTimeout = new TimeSpan(0, 0, 10, 0);
            proxy.EnableProxyTypes();

            return proxy;
        }
    }
}