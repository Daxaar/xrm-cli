using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Octono.Xrm.Tasks
{
    public class CrmTaskGateway : ITaskGateway
    {
        public Entity RetrieveWebResource(IOrganizationService service, string name)
        {
            var query = new QueryExpression("webresource") { ColumnSet = new ColumnSet(new[] { "content" }) };

            query.Criteria.AddCondition("name", ConditionOperator.Equal, name);
            var resource = service.RetrieveMultiple(query).Entities.SingleOrDefault();

            if (resource == null)
                throw new InvalidOperationException(string.Format("Cannot find JavaScript web resource {0}", name));

            return resource;
        }
    }
}