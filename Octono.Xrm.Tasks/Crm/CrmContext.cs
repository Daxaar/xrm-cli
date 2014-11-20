using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Octono.Xrm.Tasks.Crm
{
    public class CrmContext : IDisposable
    {
        public IQueryable<Entity> GetAll(string name)
        {
            return _context.CreateQuery(name);
        }

        private readonly OrganizationServiceContext _context;

        public CrmContext(IOrganizationService service)
        {
            _context = new OrganizationServiceContext(service);
        }

        public void Delete(Entity entity)
        {
            _context.DeleteObject(entity);
        }

        public void Delete(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void Add(Entity entity)
        {
            _context.DeleteObject(entity);

        }

        public void Add(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }

        }
    }
}