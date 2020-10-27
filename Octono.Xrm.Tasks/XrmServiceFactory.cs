using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Xrm.Sdk;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Holds all active connections (IOrganisationService) providing an existing or new instances to the XrmTaskContext
    /// when request by name
    /// </summary>
    public class XrmServiceFactory : IXrmServiceFactory,IDisposable
    {
        private readonly ILog _log;
        private readonly IXrmConfiguration _configuration;
        private readonly Dictionary<string,IOrganizationService> _connectionInstances = new Dictionary<string, IOrganizationService>();

        public XrmServiceFactory( ILog log, IXrmConfiguration configuration)
        {
            _log = log;
            _configuration = configuration;
        }

        public IOrganizationService Create(string connectionName)
        {
            if(string.IsNullOrEmpty(connectionName)){
                // We'll allow an empty connection name if there is only one connection defined in config
                if(_connectionInstances.Count == 1){
                    var ci = _connectionInstances.Single();
                    _log.Write("Using default connection: " + ci.Key);
                    return ci.Value;
                }
            }

            if (_connectionInstances.ContainsKey(connectionName))
            {
                return _connectionInstances[connectionName];
            }

            if (!_configuration.ConnectionStrings.ContainsKey(connectionName))
                throw new InvalidOperationException($"The connection name {connectionName} does not exist in config");

            var connection = _configuration.ConnectionStrings[connectionName];

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var orgService = OrganizationServiceFactory.Create(connection,_log);
            _connectionInstances.Add(connectionName,orgService);
            return orgService;
        }
        public void Dispose()
        {
            foreach (var service in _connectionInstances.Values.OfType<IDisposable>().Where(service => true))
            {
                service.Dispose();
            }
        }
    }
}