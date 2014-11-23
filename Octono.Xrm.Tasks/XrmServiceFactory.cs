using System;
using System.Collections.Generic;
using System.Linq;
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
            if (_connectionInstances.ContainsKey(connectionName))
            {
                return _connectionInstances[connectionName];
            }

            if (!_configuration.ConnectionStrings.ContainsKey(connectionName))
                throw new InvalidOperationException(string.Format("The connection name {0} does not exist in config", connectionName));

            var connection = _configuration.ConnectionStrings[connectionName];
            var orgService = OrganizationServiceFactory.Create(connection,_log);
            _connectionInstances.Add(connectionName,orgService);
            return orgService;
        }
        public void Dispose()
        {
            foreach (var service in _connectionInstances.Values.OfType<IDisposable>().Where(service => service != null))
            {
                service.Dispose();
            }
        }
    }
}