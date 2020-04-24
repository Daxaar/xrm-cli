using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Crm.Sdk.Messages;
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

            Console.WriteLine("creds.Windows.ClientCredential.UserName {0}",creds.Windows.ClientCredential.UserName);
            Console.WriteLine("creds.Windows.ClientCredential.Password {0}",creds.Windows.ClientCredential.Password);

            creds.UserName.UserName = connectionInfo.UserName;
            creds.UserName.Password = new NetworkCredential(string.Empty, DapiSecurePassword.Decrypt(connectionInfo.Password)).Password.Replace(":","");

            Console.WriteLine("creds.UserName.UserName {0}",creds.UserName.UserName);
            Console.WriteLine("creds.UserName.Password {0}",creds.UserName.Password);
            
            var uri = new Uri(connectionInfo.ConnectionString.Replace(@"///",@"/"));
            logger.Write("Connecting to " + uri.AbsoluteUri);

            var proxy = new OrganizationServiceProxy(uri, null, creds, null) { Timeout = new TimeSpan(0, 0, 10, 0) };
            proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.OpenTimeout = new TimeSpan(0, 0, 10, 0);
            proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.ReceiveTimeout = new TimeSpan(0, 0, 10, 0);
            proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.CloseTimeout = new TimeSpan(0, 0, 10, 0);
            proxy.ServiceConfiguration.CurrentServiceEndpoint.Binding.SendTimeout = new TimeSpan(0, 0, 10, 0);
            proxy.EnableProxyTypes();

            var response = ((WhoAmIResponse)proxy.Execute(new WhoAmIRequest()));

            return proxy;
        }
    }
}