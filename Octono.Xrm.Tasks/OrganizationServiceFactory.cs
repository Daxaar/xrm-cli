using System;
using System.ComponentModel;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Creates an OrganizationServiceProxy with the required WCF binding configuration.  Specifically a long timeout to
    /// support import/export tasks
    /// </summary>
    public class OrganizationServiceFactory
    {
        public static IOrganizationService Create(ConnectionInfo connectionInfo, ILog logger)
        {
            var creds = new ClientCredentials();
            if (!string.IsNullOrEmpty(connectionInfo.UserName) && !string.IsNullOrEmpty(connectionInfo.Password))
            {
                creds.Windows.ClientCredential = new NetworkCredential(connectionInfo.UserName,DapiSecurePassword.Decrypt(connectionInfo.Password));   
            }
            else
            {
                creds.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;                
            }

            creds.UserName.UserName = connectionInfo.UserName;
            creds.UserName.Password = new NetworkCredential(string.Empty, DapiSecurePassword.Decrypt(connectionInfo.Password)).Password.Replace(":","");

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